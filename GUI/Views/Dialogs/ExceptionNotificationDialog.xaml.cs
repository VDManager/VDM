using System;
using System.Windows;

using VDM.GUI.Interfaces;



namespace VDM.GUI.Views.Dialogs
{
    public partial class ExceptionNotificationDialog : Window, IClosable
    {
        public ExceptionNotificationDialog()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception) { } // Do not throw exceptions from this window, since it's job is to show them
        }
    }
}
