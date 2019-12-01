using VDM.Data.Model;



namespace VDM.GUI.ViewModels.Base
{
    public static class Messenger
    {
        public delegate void CloseAppHandler();

        public static event CloseAppHandler CloseApp;

        public static void RegisterCloseAppHandler(CloseAppHandler handler)
        {
            if (CloseApp == null) // Only allow to register MainWindowVM
                CloseApp += handler;
        }

        public static void SignalCloseApp()
        {
            CloseApp?.Invoke();
        }



        public delegate void OpenNewHandler();

        public static event OpenNewHandler OpenNew;

        public static void RegisterOpenNewHandler(OpenNewHandler handler)
        {
            if (OpenNew == null) // Only allow to register MainWindowVM
                OpenNew += handler;
        }

        public static void SignalOpenNew()
        {
            OpenNew?.Invoke();
        }



        public delegate void OpenEditHandler(VirtualDesktopPreset vdp);

        public static event OpenEditHandler OpenEdit;

        public static void RegisterOpenEditHandler(OpenEditHandler handler)
        {
            if (OpenEdit == null) // Only allow to register MainWindowVM
                OpenEdit += handler;
        }

        public static void SignalOpenEdit(VirtualDesktopPreset vdp)
        {
            OpenEdit?.Invoke(vdp);
        }



        public delegate void ReturnToListHandler(bool shouldRefreshList);

        public static event ReturnToListHandler ReturnToList;

        public static void RegisterReturnToListHandler(ReturnToListHandler handler)
        {
            if (ReturnToList == null) // Only allow to register MainWindowVM
                ReturnToList += handler;
        }

        public static void SignalReturnToList(bool shouldRefreshList)
        {
            ReturnToList?.Invoke(shouldRefreshList);
        }
    }
}
