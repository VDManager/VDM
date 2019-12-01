using System.Windows.Input;

using VDM.GUI.Interfaces;
using VDM.GUI.ViewModels.Base;



namespace VDM.GUI.ViewModels.Dialogs
{
    public class ExceptionNotificationViewModel : ViewModelBase
    {
        public ExceptionNotificationViewModel(string title, string description, string details = null)
        {
            CloseCommand = new RelayCommand<IClosable>(exe => ExecuteClose(exe), can => CanExecuteClose);

            Title = title ?? "Unknown error";
            Description = description ?? "We don't know what went wrong. Please restart the application!";
            Details = details ?? "";
        }



        public ICommand CloseCommand { get; }



        private string title;
        public string Title
        {
            get => title;
            private set => SetProperty(ref title, value);
        }

        private string description;
        public string Description
        {
            get => description;
            private set => SetProperty(ref description, value);
        }

        private string details;
        public string Details
        {
            get => details;
            private set => SetProperty(ref details, value);
        }



        private bool CanExecuteClose => true;
        private void ExecuteClose(IClosable window)
        {
            window?.Close();
        }
    }
}
