using System.ComponentModel;

using GUI.Interfaces;
using VDM.Data.Model;



namespace VDM.GUI.ModelExtensions
{
    public class SelectableVDP : VirtualDesktopPreset, ISelectable, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool isSelected;
        public bool IsSelected
        {
            get => isSelected;
            set
            {
                if (isSelected != value)
                {
                    isSelected = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsSelected)));
                }
            }
        }
    }
}
