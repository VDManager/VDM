using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;



namespace VDM.GUI.ViewModels.Base
{
    /// <summary>
    /// Encapsulates INPC implementation, that is common for all ViewModels.
    /// </summary>
    public class ViewModelBase : INotifyPropertyChanged
    {
        /// <summary>
        /// Notifies the Views about every change in the VMs. 
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets called in every property's set() method, overwrites storage value (if newer) and notifies Views.
        /// </summary>
        /// <typeparam name="T">Type of property to set.</typeparam>
        /// <param name="storage">Propery backing storage as a reference.</param>
        /// <param name="value">New property value.</param>
        /// <param name="propertyName">Name of the property to set.</param>
        protected virtual void SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(storage, value))
                return;

            storage = value;
            NotifyPropertyChanged(propertyName);
        }

        /// <summary>
        /// Notifies Views about a value change.
        /// </summary>
        /// <param name="propertyName">Name of the property which changed.</param>
        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
