using System;
using System.IO;

namespace ManagedBass
{
    /// <summary>
    /// Wraps Plugins AddOns.
    /// </summary>
    public class Plugin
    {
        public string DllName { get; }
        int _hPlugin;
        PluginInfo? _info;

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

        internal Plugin(string DllName) { this.DllName = DllName; }

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

        public void Unload()
        {
            if (_hPlugin != 0 && Bass.PluginFree(_hPlugin))
                _hPlugin = 0;
        }
    }
}
