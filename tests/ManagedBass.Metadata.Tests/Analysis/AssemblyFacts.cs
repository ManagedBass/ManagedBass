using System.Collections.Immutable;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;

namespace ManagedBass.Metadata.Tests.Analysis;

public sealed record DllImportSite(string DeclaringType, string Method, string Module)
{
    public override string ToString() => $"{DeclaringType}.{Method} -> \"{Module}\"";
}

public sealed record PublicTypeFacts(bool Exists, bool IsPublic, IReadOnlyList<string> PublicStaticMethods)
{
    public IReadOnlyList<string> MissingMembers(IEnumerable<string> required) =>
        Exists && IsPublic ? required.Except(PublicStaticMethods, StringComparer.Ordinal).ToArray() : required.ToArray();
}

/// <summary>Reads what a built assembly actually declares, straight from its metadata tables.</summary>
public static class AssemblyFacts
{
    public static IReadOnlyList<DllImportSite> DllImports(byte[] image)
    {
        using var pe = new PEReader(ImmutableArray.Create(image));
        var md = pe.GetMetadataReader();

        return md.MethodDefinitions
            .Select(md.GetMethodDefinition)
            .Where(m => (m.Attributes & MethodAttributes.PinvokeImpl) != 0)
            .Select(m => new DllImportSite(
                FullName(md, md.GetTypeDefinition(m.GetDeclaringType())),
                md.GetString(m.Name),
                md.GetString(md.GetModuleReference(m.GetImport().Module).Name)))
            .OrderBy(s => s.DeclaringType, StringComparer.Ordinal)
            .ThenBy(s => s.Method, StringComparer.Ordinal)
            .ToArray();
    }

    public static IReadOnlyList<string> DistinctModules(byte[] image) =>
        DllImports(image).Select(s => s.Module).Distinct(StringComparer.Ordinal).OrderBy(m => m, StringComparer.Ordinal).ToArray();

    public static PublicTypeFacts PublicType(byte[] image, string fullName)
    {
        using var pe = new PEReader(ImmutableArray.Create(image));
        var md = pe.GetMetadataReader();

        var types = md.TypeDefinitions.Select(md.GetTypeDefinition).Where(t => FullName(md, t) == fullName).ToArray();

        if (types.Length == 0)
            return new(false, false, []);

        var type = types[0];
        var isPublic = (type.Attributes & TypeAttributes.VisibilityMask) == TypeAttributes.Public;

        var methods = type.GetMethods()
            .Select(md.GetMethodDefinition)
            .Where(m => (m.Attributes & MethodAttributes.MemberAccessMask) == MethodAttributes.Public && (m.Attributes & MethodAttributes.Static) != 0)
            .Select(m => Signature(md, m))
            .Distinct(StringComparer.Ordinal)
            .OrderBy(s => s, StringComparer.Ordinal)
            .ToArray();

        return new(true, isPublic, methods);
    }

    private static string Signature(MetadataReader md, MethodDefinition method)
    {
        var signature = method.DecodeSignature(new TypeNameProvider(), null);
        return $"{md.GetString(method.Name)}({string.Join(", ", signature.ParameterTypes)})";
    }

    private static string FullName(MetadataReader md, TypeDefinition type)
    {
        var name = md.GetString(type.Name);
        var ns = md.GetString(type.Namespace);

        if (type.GetDeclaringType().IsNil)
            return ns.Length == 0 ? name : $"{ns}.{name}";

        return $"{FullName(md, md.GetTypeDefinition(type.GetDeclaringType()))}+{name}";
    }

    // Only what is needed to render parameter types by full name; everything exotic degrades to a marker.
    private sealed class TypeNameProvider : ISignatureTypeProvider<string, object?>
    {
        public string GetPrimitiveType(PrimitiveTypeCode typeCode) => typeCode switch
        {
            PrimitiveTypeCode.Boolean => "System.Boolean",
            PrimitiveTypeCode.Int32 => "System.Int32",
            PrimitiveTypeCode.String => "System.String",
            PrimitiveTypeCode.Object => "System.Object",
            PrimitiveTypeCode.Void => "System.Void",
            PrimitiveTypeCode.IntPtr => "System.IntPtr",
            _ => $"System.{typeCode}"
        };

        public string GetTypeFromDefinition(MetadataReader reader, TypeDefinitionHandle handle, byte rawTypeKind) =>
            FullName(reader, reader.GetTypeDefinition(handle));

        public string GetTypeFromReference(MetadataReader reader, TypeReferenceHandle handle, byte rawTypeKind)
        {
            var reference = reader.GetTypeReference(handle);
            var ns = reader.GetString(reference.Namespace);
            var name = reader.GetString(reference.Name);
            return ns.Length == 0 ? name : $"{ns}.{name}";
        }

        public string GetTypeFromSpecification(MetadataReader reader, object? context, TypeSpecificationHandle handle, byte rawTypeKind) => "?spec";
        public string GetSZArrayType(string elementType) => $"{elementType}[]";
        public string GetArrayType(string elementType, ArrayShape shape) => $"{elementType}[{new string(',', shape.Rank - 1)}]";
        public string GetByReferenceType(string elementType) => $"{elementType}&";
        public string GetPointerType(string elementType) => $"{elementType}*";
        public string GetGenericInstantiation(string genericType, ImmutableArray<string> typeArguments) => $"{genericType}<{string.Join(", ", typeArguments)}>";
        public string GetGenericMethodParameter(object? context, int index) => $"!!{index}";
        public string GetGenericTypeParameter(object? context, int index) => $"!{index}";
        public string GetFunctionPointerType(MethodSignature<string> signature) => "?fnptr";
        public string GetModifiedType(string modifier, string unmodifiedType, bool isRequired) => unmodifiedType;
        public string GetPinnedType(string elementType) => elementType;
    }
}
