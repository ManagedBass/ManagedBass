using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ManagedBass
{
    /// <summary>
    /// Base implementation of <see cref="INotifyPropertyChanged"/>
    /// </summary>
    public class INPC : INotifyPropertyChanged
    {
        /// <summary>
        /// Fired when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        
        /// <summary>
        /// Fires the <see cref="PropertyChanged"/> event.
        /// </summary>
        protected virtual void OnPropertyChanged([CallerMemberName] string PropertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
    }
}
