using System;
using System.IO;

namespace ManagedBass
{
    /// <summary>
    /// Wraps Plugins AddOns.
    /// </summary>
    public class Plugin
    {
        /// <summary>
        /// FileName of the Plugin.
        /// </summary>
        public string DllName { get; }

        int _hPlugin;
        PluginInfo? _info;

        /// <summary>
        /// Gets the Version of the Plugin.
        /// </summary>
        public Version Version
        {
            get
            {
                if (_info != null)
                    return _info.Value.Version;

                Load();

                _info = Bass.PluginGetInfo(_hPlugin);

                return _info.Value.Version;
            }
        }

        /// <summary>
        /// Gets the Formats supported by the Plugin.
        /// </summary>
        public PluginFormat[] SupportedFormats
        {
            get
            {
                if (_info == null)
                {
                    Load();

                    _info = Bass.PluginGetInfo(_hPlugin);
                }

                return _info.Value.Formats;
            }
        }

        /// <summary>
        /// Creates a new instance of plugin.
        /// </summary>
        /// <param name="DllName">Name of the library without extension or lib prefix.</param>
        public Plugin(string DllName) { this.DllName = DllName; }

        /// <summary>
        /// Load the plugin into memory.
        /// <param name="Folder">Folder to load the plugin from... <see langword="null"/> (default), Load from Current Directory.</param>
        /// </summary>
        public void Load(string Folder = null)
        {
            if (_hPlugin != 0)
                return;

            _hPlugin = Bass.PluginLoad(Folder != null ? Path.Combine(Folder, DllName) : DllName);
            
            if (_hPlugin == 0)
                throw new DllNotFoundException(DllName);
        }

        /// <summary>
        /// Unloads the Plugin from Memory
        /// </summary>
        public void Unload()
        {
            if (_hPlugin != 0 && Bass.PluginFree(_hPlugin))
                _hPlugin = 0;
        }
    }
}
