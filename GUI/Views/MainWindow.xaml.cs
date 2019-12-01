using System;
using System.Windows;

using VDM.GUI.Helpers;
using VDM.GUI.Interfaces;
using VDM.GUI.ViewModels;


namespace VDM.GUI.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IClosable
    {
        public MainWindow(MainWindowViewModel vm)
        {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(App_ExceptionHandler);
            DataContext = vm;
            InitializeComponent();
        }

        private void App_ExceptionHandler(object sender, UnhandledExceptionEventArgs args)
        {
            Exception ex = (Exception)args.ExceptionObject;

            ExceptionHandler.ShowExceptionNotification("Unhandled exception in VDM!", ex.Message, $"Stacktrace:\n{ex.StackTrace}", this);
        }
    }
}
