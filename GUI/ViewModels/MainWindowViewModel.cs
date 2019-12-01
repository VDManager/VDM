using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using VDM.GUI.ViewModels.Factories;
using VDM.GUI.Helpers;
using VDM.GUI.ViewModels.Base;
using VDM.GUI.Views;
using VDM.Data.Model;
using VDM.Services.Interfaces;


namespace VDM.GUI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private List<VirtualDesktopPreset> virtualDesktopPresets;

        private readonly IDatabaseService dbService;
        private readonly IPresetManager presetManager;
        private readonly IVirtualDesktopService vdService;
        private readonly ListViewModelFactory listViewModelFactory;
        private readonly VDPDetailsViewModelFactory vdpViewModelFactory;


        public MainWindowViewModel(IDatabaseService dbService, IVirtualDesktopService vdService,
            IPresetManager presetManager, ListViewModelFactory listViewModelFactory,
            VDPDetailsViewModelFactory vdpViewModelFactory) : base()
        {
            this.presetManager = presetManager;
            this.dbService = dbService;
            this.vdService = vdService;
            this.listViewModelFactory = listViewModelFactory;
            this.vdpViewModelFactory = vdpViewModelFactory;

            OnLoadedCommand = new RelayCommand<Window>(window => ExecuteOnLoaded(window));

            virtualDesktopPresets = dbService.GetAllPresets().Result;

            Title = "Virtual Desktop Manager";
        }

        public ICommand OnLoadedCommand { get; }

        private string title;

        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        private object content;

        public object Content
        {
            get => content;
            set => SetProperty(ref content, value);
        }


        private void ExecuteOnLoaded(Window window)
        {
            CheckStartupSuccess(window);

            RegisterEventHandlers(window);

            SwitchToListContent(false);
        }


        private void RegisterEventHandlers(Window window)
        {
            Messenger.RegisterCloseAppHandler(() => { window?.Close(); });

            Messenger.RegisterOpenEditHandler(SwitchToEditContent);
            Messenger.RegisterOpenNewHandler(SwitchToNewContent);
            Messenger.RegisterReturnToListHandler(SwitchToListContent);
        }

        private async void SwitchToListContent(bool shouldRefreshList)
        {
            if (shouldRefreshList)
                virtualDesktopPresets = await dbService.GetAllPresets();

            var listView = new ListView();
            listView.DataContext = listViewModelFactory.Build(virtualDesktopPresets);

            Content = listView;

            Title = "Virtual Desktop Manager";
        }

        private void SwitchToNewContent()
        {
            var detailsView = new VDPDetailsView(vdpViewModelFactory);
            detailsView.DataContext = vdpViewModelFactory.Build();

            Content = detailsView;

            Title += " - new VDP";
        }

        private void SwitchToEditContent(VirtualDesktopPreset vdp)
        {
            var detailsView = new VDPDetailsView(vdpViewModelFactory);
            detailsView.DataContext = vdpViewModelFactory.Build(vdp);

            Content = detailsView;

            Title += " - edit VDP";
        }

        private void CheckStartupSuccess(Window window)
        {
            if (!vdService.IsSupported)
                ExceptionHandler.ShowExceptionNotification("VD management not supported!",
                    "The Virtual Desktop manager service cannot be initialized or is not supported in this environment.",
                    null, window);
        }
    }
}