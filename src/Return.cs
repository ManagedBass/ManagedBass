using ManagedBass.Dynamics;

namespace ManagedBass
{
    // TODO: Manage Marshaling of Value excluding ErrorCode

    /// <summary>
    /// Wraps a Bass Error in a function return value
    /// </summary>
    /// <typeparam name="T">The Type of the function return value</typeparam>
    public class Return<T>
    {
        public Errors ErrorCode { get; }

        Return(T Value)
        {
            ErrorCode = Bass.LastError;
            this.Value = Value;
        }

        public T Value { get; }

        public static implicit operator T(Return<T> e) => e.Value;

        public static implicit operator Return<T>(T e) => new Return<T>(e);

        public bool Success => ErrorCode == Errors.OK;
    }
}
