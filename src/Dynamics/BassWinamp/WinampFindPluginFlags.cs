using System;

namespace ManagedBass.Dynamics
{
    /// <summary>
    /// Flags to be used with <see cref="BassWinamp.FindPlugins" />.
    /// </summary>
    [Flags]
    public enum WinampFindPluginFlags
    {
        /// <summary>
        /// Find input plug-ins. Should and must always be specified.
        /// </summary>
        Input = 1,

        /// <summary>
        /// Recursively loop through all sub-directories as well.
        /// </summary>
        Recursive = 4,

        /// <summary>
        /// Return the result as a comma seperated list in the format: item1,item2,"item with , commas",item4,"item with space"
        /// <para>If not specified a list of null-terminated Ansi strings will be returned ending with a double-null.</para>
        /// </summary>
        CommaList = 8
    }
}