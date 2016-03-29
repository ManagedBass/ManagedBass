using System;
using System.IO;
using System.Linq;

namespace ManagedBass
{
    /// <summary>
    /// Wraps AddOns that are no more than Plugins.
    /// </summary>
    /// 
    /// <remarks>
    /// Currently Wraps: BassAAC, BassAC3, BassADX, BassAIX, BassALAC, BassAPE, BassFLAC, BassHLS, BassMPC, BassOFR, BassOPUS, BassSPX, BassTTA, BassWV
    /// </remarks>
    public class Plugin
    {
        public string DllName { get; }
        int HPlugin;
        PluginInfo? info;

        public Version Version
        {
            get
            {
                if (info != null)
                    return info.Value.Version;

                Load();

                info = Bass.PluginGetInfo(HPlugin);

                return info.Value.Version;
            }
        }

        public PluginFormat[] SupportedFormats
        {
            get
            {
                if (info == null)
                {
                    Load();

                    info = Bass.PluginGetInfo(HPlugin);
                }

                return info.Value.Formats;
            }
        }

        /// <summary>
        /// BassAc3 AddOn: Dynamic Range Compression (default is false).
        /// </summary>
        public static bool AC3_DRC
        {
            get 
            {
                BassAC3.Load();

                return Bass.GetConfigBool(Configuration.AC3DynamicRangeCompression); 
            }
            set 
            {
                BassAC3.Load();

                Bass.Configure(Configuration.AC3DynamicRangeCompression, value); 
            }
        }

        Plugin(string DllName) { this.DllName = DllName; }

        /// <summary>
        /// Load the plugin into memory.
        /// <param name="Folder">Folder to load the plugin from... <see langword="null"/> (default), Load from Current Directory.</param>
        /// </summary>
        public void Load(string Folder = null)
        {
            if (HPlugin != 0)
                return;

            // Try for Windows, Linux/Android and OSX Libraries respectively.
            HPlugin = new[] { DllName + ".dll", "lib" + DllName + ".so", "lib" + DllName + ".dylib" }
                              .Select(lib => Folder != null ? Path.Combine(Folder, lib) : lib)
                              .Select(Bass.PluginLoad)
                              .FirstOrDefault(h => h != 0);
            
            if (HPlugin == 0)
                throw new DllNotFoundException(DllName);

            // Always Support MP4 files in BassAAC.CreateStream()
            if (DllName == BassAAC.DllName)
                Bass.Configure(Configuration.AacSupportMp4, true);
        }

        public void Unload()
        {
            if (HPlugin != 0 && Bass.PluginFree(HPlugin))
                HPlugin = 0;
        }

        #region Instances
        /// <summary>
        /// Wraps BassAAC: bass_aac.dll.
        /// </summary>
        /// <remarks>
        /// MP4 and AAC both are always supported.
        /// <para>Supports .aac, .adts, .mp4, .m4a, .m4b</para>
        /// </remarks>
        public static readonly Plugin BassAAC = new Plugin("bass_aac");

        /// <summary>
        /// Wraps BassAC3: bassac3.dll
        /// </summary>
        /// <remarks>
        /// Supports: .ac3
        /// </remarks>
        public static readonly Plugin BassAC3 = new Plugin("bass_ac3");

        /// <summary>
        /// Wraps BassADX: bassadx.dll
        /// </summary>
        /// <remarks>
        /// Supports: .adx
        /// </remarks>
        public static readonly Plugin BassADX = new Plugin("bass_adx");

        /// <summary>
        /// Wraps BassAIX: bass_aix.dll
        /// </summary>
        public static readonly Plugin BassAIX = new Plugin("bass_aix");

        /// <summary>
        /// Wraps BassALAC: bassalac.dll
        /// </summary>
        /// <remarks>
        /// Supports: .m4a, .aac, .mp4, .mov
        /// </remarks>
        public static readonly Plugin BassALAC = new Plugin("bassalac");

        /// <summary>
        /// Wraps BassAPE: bass_ape.dll
        /// </summary>
        /// <remarks>
        /// Supports: .ape, .ap1
        /// </remarks>
        public static readonly Plugin BassAPE = new Plugin("bass_ape");

        /// <summary>
        /// Wraps BassFLAC: bassflac.dll
        /// </summary>
        /// <remarks>
        /// Supports: .flac
        /// </remarks>
        public static readonly Plugin BassFLAC = new Plugin("bassflac");

        /// <summary>
        /// Wraps BassHLS: basshls.dll
        /// </summary>
        public static readonly Plugin BassHLS = new Plugin("basshls");

        /// <summary>
        /// Wraps BassMPC: bass_mpc.dll
        /// </summary>
        /// <remarks>
        /// Supports: .mpc, .mpp, .mp+
        /// </remarks>
        public static readonly Plugin BassMPC = new Plugin("bass_mpc");

        /// <summary>
        /// Wraps BassOFR: bass_ofr.dll
        /// </summary>
        /// <remarks>
        /// Supports: .ofr, .ofs
        /// </remarks>
        public static readonly Plugin BassOFR = new Plugin("bass_ofr");

        /// <summary>
        /// Wraps BassOPUS: bassopus.dll
        /// </summary>
        /// <remarks>
        /// Supports: .opus
        /// </remarks>
        public static readonly Plugin BassOPUS = new Plugin("bassopus");

        /// <summary>
        /// Wraps BassSPX: bass_spx.dll
        /// </summary>
        /// <remarks>
        /// Supports: .spx
        /// </remarks>
        public static readonly Plugin BassSPX = new Plugin("bass_spx");

        /// <summary>
        /// Wraps BassTTA: bass_tta.dll
        /// </summary>
        /// <remarks>
        /// Supports: .tta
        /// </remarks>
        public static readonly Plugin BassTTA = new Plugin("bass_tta");

        /// <summary>
        /// Wraps BassWV: basswv.dll
        /// </summary>
        /// <remarks>
        /// Supports: .wv
        /// </remarks>
        public static readonly Plugin BassWV = new Plugin("basswv");
        #endregion
    }
}
