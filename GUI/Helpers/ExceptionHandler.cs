using System;
using System.Diagnostics;
using System.Runtime.ExceptionServices;
using System.Windows;

using VDM.GUI.ViewModels.Dialogs;
using VDM.GUI.Views.Dialogs;



namespace VDM.GUI.Helpers
{
    /// <summary>
    /// Provides exception and error handling functions.
    /// </summary>
    public static class ExceptionHandler
    {
        /// <summary>
        /// Notifies the user about an exception that happened during operation.
        /// If this process also raises an exception, the app will quit.
        /// </summary>
        /// <param name="title">Title of the notification dialog.</param>
        /// <param name="description">Short and user-friendly descrition of the exception.</param>
        /// <param name="details">More elaborate exception details.</param>
        /// <param name="owner">The parent window upon which to open the dialog (if any).</param>
        public static void ShowExceptionNotification(string title, string description, string details, Window owner = null)
        {
            try
            {
                ExceptionNotificationDialog enDialog = new ExceptionNotificationDialog();
                ExceptionNotificationViewModel enVM = new ExceptionNotificationViewModel(title, description, details);

                enDialog.DataContext = enVM;
                if (owner != null)
                    enDialog.Owner = owner;

                enDialog.ShowDialog();
            }
            catch (Exception) // Do not throw exceptions from this window, since it's job is to show them
            {
                try
                {
                    // However we try a simpler method to at least tell the user that the app will close
                    MessageBox.Show("An unknown error occured during the handling of another error. The application will now exit. Sorry! :(",
                        "Unknown error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception) { } // If even that fails, nothing to do...
                finally
                {
                    Application.Current.Dispatcher.BeginInvoke((Action)(() =>
                    {
                        Application.Current.Shutdown(); // Trying to manually close the application in a thread-safe manner
                    }));
                }
            }
        }

        /// <summary>
        /// Notifies the user about an exception that happened during operation.
        /// If this process also raises an exception, the app will quit.
        /// This overload might be best used only in debug mode, since it will print out the stack trace.
        /// </summary>
        /// <param name="ex">Exception object to show info about.</param>
        /// <param name="owner">The parent window upon which to open the dialog (if any).</param>
        public static void ShowExceptionNotification(Exception ex, Window owner = null)
        {
            ShowExceptionNotification($"{ex.GetType().Name} error!", ex.Message, $"Stacktrace:\n{ex.StackTrace}", owner);
        }

        /// <summary>
        /// Swallows all exceptions, rethrowing only if we're in debug mode (keeping the original stack trace intact).
        /// </summary>
        /// <param name="ex">The exception to swallow or rethrow.</param>
        public static void RethrowExceptionIfDebugging(Exception ex)
        {
            if (Debugger.IsAttached)
            {
                // Keeping the stack trace by using this solution, because we cannot implicitly rethrow from another method
                ExceptionDispatchInfo.Capture(ex).Throw();
            }
        }
    }
}
