using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace ManagedBass.Dynamics
{
    public static partial class Bass
    {
        /// <summary>
		/// Retrieves information on a plugin.
		/// </summary>
		/// <param name="Handle">The plugin handle - or 0 to retrieve native BASS information.</param>
		/// <returns>An instance of <see cref="PluginInfo" /> is returned. Use <see cref="LastError" /> to get the error code.</returns>
		/// <remarks>The plugin information does not change, so the returned info remains valid for as long as the plugin is loaded.
		/// <para>Note: There is no guarantee that the check is complete or might contain formats not being supported on your particular OS/machine (due to additional or missing audio codecs).</para>
		/// </remarks>
        /// <exception cref="Errors.InvalidHandle"><paramref name="Handle" /> is not valid.</exception>
        [DllImport(DllName, EntryPoint = "BASS_PluginGetInfo")]
        public static extern PluginInfo PluginGetInfo(int Handle);
        
        #region PluginLoad
        [DllImport(DllName, CharSet = CharSet.Unicode)]
        static extern int BASS_PluginLoad(string FileName, BassFlags Flags = BassFlags.Unicode);
        
		/// <summary>
		/// Plugs on "add-on" into the standard stream and sample creation functions.
		/// </summary>
		/// <param name="FileName">Filename of the add-on/plugin.</param>
		/// <returns>If successful, the loaded plugin's handle is returned, else 0 is returned. Use <see cref="LastError" /> to get the error code.</returns>
		/// <remarks>
		/// <para>
        /// There are 2 ways in which add-ons can provide support for additional formats.
        /// They can provide dedicated functions to create streams of the specific format(s) they support and/or they can plug into the standard stream creation functions: 
		/// <see cref="CreateStream(string,long,long,BassFlags)" />, <see cref="CreateStream(string,int,BassFlags,DownloadProcedure,IntPtr)" />,
        /// and <see cref="CreateStream(StreamSystem,BassFlags,FileProcedures,IntPtr)" />.
        /// This function enables the latter method.
        /// Both methods can be used side by side. 
		/// The obvious advantage of the plugin system is convenience, while the dedicated functions can provide extra options that are not possible via the shared function interfaces.
        /// See an add-on's documentation for more specific details on it.
        /// </para>
		/// <para>As well as the stream creation functions, plugins also add their additional format support to <see cref="SampleLoad(string,long,int,int,BassFlags)" />.</para>
		/// <para>Information on what file formats a plugin supports is available via the <see cref="PluginGetInfo" /> function.</para>
		/// <para>
        /// When using multiple plugins, the stream/sample creation functions will try each of them in the order that they were loaded via this function, until one that accepts the file is found.
		/// When an add-on is already loaded (eg. if you are using functions from it), the plugin system will use the same instance (the reference count will just be incremented); there will not be 2 copies of the add-on in memory.
        /// </para>
		/// <para>Note: Only stream/music add-ons are loaded (e.g. <see cref="BassFx"/> or <see cref="BassMix"/> are NOT loaded).</para>
		/// <para><b>Platform-specific:</b></para>
		/// <para>
        /// Dynamic libraries are not permitted on iOS, so add-ons are provided as static libraries instead, which means this function has to work a little differently.
        /// The add-on needs to be linked into the executable, and a "plugin" symbol declared and passed to this function (instead of a filename).
        /// </para>
		/// </remarks>
        /// <exception cref="Errors.FileOpen">The <paramref name="FileName" /> could not be opened.</exception>
        /// <exception cref="Errors.UnsupportedFileFormat">The <paramref name="FileName" /> is not a plugin.</exception>
        /// <exception cref="Errors.Already">The <paramref name="FileName" /> is already plugged in.</exception>
        public static int PluginLoad(string FileName) => BASS_PluginLoad(FileName);
        #endregion

        /// <summary>
		/// Unplugs an add-on.
		/// </summary>
		/// <param name="Handle">The plugin handle... 0 = all plugins.</param>
		/// <returns>If successful, <see langword="true" /> is returned, else <see langword="false" /> is returned. Use <see cref="LastError" /> to get the error code.</returns>
		/// <remarks>
        /// If there are streams created by a plugin in existence when it is being freed, the streams will automatically be freed too.
        /// Samples loaded by the plugin are unaffected as the plugin has nothing to do with them once they are loaded (the sample data is already fully decoded).
		/// </remarks>
        /// <exception cref="Errors.InvalidHandle"><paramref name="Handle" /> is not valid.</exception>
        [DllImport(DllName, EntryPoint = "BASS_PluginFree")]
        public static extern bool PluginFree(int Handle);

        /// <summary>
		/// Loads all BASS add-ons (bass*.dll or libbass*.so or libbass*.dylib) contained in the specified directory.
		/// </summary>
		/// <param name="directory">The directory in which to search for BASS add-ons.</param>
		/// <returns>A Dictionary of filename and plugin handle.</returns>
		public static Dictionary<string, int> PluginLoadDirectory(string directory)
        {
            var list = new Dictionary<string, int>();

            if (Directory.Exists(directory))
            {
                foreach (var wildcard in new[] { "bass*.dll", "libbass*.so", "libbass*.dylib" })
                {
                    var libs = Directory.GetFiles(directory, wildcard);

                    if (libs == null || libs.Length == 0)
                        continue;

                    foreach (var lib in libs)
                    {
                        int h = BASS_PluginLoad(lib);
                        if (h != 0) list.Add(lib, h);
                    }

                    if (list.Count > 0)
                        break;
                }
            }

            return list;
        }
    }
}
