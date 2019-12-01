using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

using VDM.GUI.ModelExtensions;
using VDM.GUI.ViewModels.Base;
using VDM.GUI.Helpers;
using VDM.Data.Model;
using VDM.Services.Interfaces;



namespace VDM.GUI.ViewModels
{
    public class ListViewModel : ViewModelBase
    {
        private static bool isFirstRun = true;

        private List<SelectableVDP> selectedVDPItems;

        private readonly IDatabaseService dbService;
        private readonly IPresetManager presetManager;


        #region Constructor and ICommand props

        public ListViewModel(IDatabaseService dbService, IPresetManager presetManager) : this(null, dbService, presetManager) { }

        public ListViewModel(List<VirtualDesktopPreset> vdpList, IDatabaseService dbService, IPresetManager presetManager)
        {
            this.dbService = dbService;
            this.presetManager = presetManager;

            CloseCommand = new RelayCommand<object>(x => ExecuteClose());
            NewVDPCommand = new RelayCommand<object>(x => ExecuteNewVDP(), c => CanExecuteNewVDP);
            EditVDPCommand = new RelayCommand<object>(x => ExecuteEditVDP(), c => CanExecuteEditVDP);
            OpenVDPsCommand = new RelayCommand<object>(x => ExecuteOpenVDPs(), c => CanExecuteOpenVDPs);
            SelectAllVDPsCommand = new RelayCommand<object>(x => ExecuteSelectAllVDPs(), c => CanExecuteSelectAllVDPs);
            UnselectAllVDPsCommand = new RelayCommand<object>(x => ExecuteUnselectAllVDPs(), c => CanExecuteUnselectAllVDPs);
            ReverseVDPSelectionCommand = new RelayCommand<object>(x => ExecuteReverseVDPSelection(), c => CanExecuteReverseVDPSelection);
            DeleteVDPsCommand = new RelayCommand<object>(x => ExecuteDeleteVDPs(), c => CanExecuteDeleteVDPs);
            OpenDocumentationCommand = new RelayCommand<object>(x => ExecuteOpenDocumentation());
            OpenAboutCommand = new RelayCommand<object>(x => ExecuteOpenAbout());
            ListSelectedChangedCommand = new RelayCommand<object>(x => ExecuteListSelectedChanged());

            VDPItems = new ObservableCollection<SelectableVDP>();
            selectedVDPItems = new List<SelectableVDP>();

            foreach (var vdp in vdpList)
            {
                VDPItems.Add(Converters.VDPModelToGUI(vdp));
            }

            if (isFirstRun)
                OpenPresetsOnStartup();
        }

        public ICommand CloseCommand { get; }
        public ICommand NewVDPCommand { get; }
        public ICommand EditVDPCommand { get; }
        public ICommand OpenVDPsCommand { get; }
        public ICommand SelectAllVDPsCommand { get; }
        public ICommand UnselectAllVDPsCommand { get; }
        public ICommand ReverseVDPSelectionCommand { get; }
        public ICommand DeleteVDPsCommand { get; }
        public ICommand OpenDocumentationCommand { get; }
        public ICommand OpenAboutCommand { get; }
        public ICommand ListSelectedChangedCommand { get; }

        #endregion

        #region Command implementations

        private ObservableCollection<SelectableVDP> items;
        public ObservableCollection<SelectableVDP> VDPItems
        {
            get => items;
            set => SetProperty(ref items, value);
        }

        private void ExecuteClose()
        {
            Messenger.SignalCloseApp();
        }

        private bool CanExecuteNewVDP => VDPItems.Count < 64;
        private void ExecuteNewVDP()
        {
            Messenger.SignalOpenNew();
        }

        public bool CanExecuteEditVDP => selectedVDPItems.Count == 1;
        private void ExecuteEditVDP()
        {
            Messenger.SignalOpenEdit(selectedVDPItems[0]);
        }

        public bool CanExecuteOpenVDPs => selectedVDPItems.Count > 0;
        private void ExecuteOpenVDPs()
        {
            presetManager.OpenPresetList(selectedVDPItems.Select(a => a as VirtualDesktopPreset).ToList());
        }

        public bool CanExecuteSelectAllVDPs => VDPItems.Count > 0;
        private void ExecuteSelectAllVDPs()
        {
            foreach (var vdp in VDPItems)
            {
                vdp.IsSelected = true;
            }
        }

        public bool CanExecuteUnselectAllVDPs => selectedVDPItems.Count > 0;
        private void ExecuteUnselectAllVDPs()
        {
            foreach (var vdp in VDPItems)
            {
                vdp.IsSelected = false;
            }
        }

        public bool CanExecuteReverseVDPSelection => selectedVDPItems.Count > 0;
        private void ExecuteReverseVDPSelection()
        {
            foreach (var vdp in VDPItems)
            {
                vdp.IsSelected = !vdp.IsSelected;
            }
        }

        public bool CanExecuteDeleteVDPs => selectedVDPItems.Count > 0;
        private void ExecuteDeleteVDPs()
        {
            if (MessageBoxResult.Yes == MessageBox.Show("This action is permanent and cannot be undone.", "Are you sure?", MessageBoxButton.YesNo, MessageBoxImage.Warning))
            {
                foreach (var vdp in selectedVDPItems)
                {
                    dbService.RemovePreset(vdp);
                    VDPItems.Remove(vdp);
                }

                selectedVDPItems.Clear();

                NotifyPropertyChanged(nameof(VDPItems));
            }
        }

        private void ExecuteOpenDocumentation()
        {
            System.Diagnostics.ProcessStartInfo procStartInfo = new System.Diagnostics.ProcessStartInfo()
            {
                FileName = "Readme.pdf",
                UseShellExecute = true,
                Verb = "open"
            };

            try
            {
                System.Diagnostics.Process.Start(procStartInfo);
            }
            catch (Exception)
            {
                ExceptionHandler.ShowExceptionNotification("Readme.pdf not found",
                    "Cannot open documentation, since the file Readme.pdf was not found.", null);
            }
        }

        private void ExecuteOpenAbout()
        {
            MessageBox.Show("DIM\nGM", "VDM - Version 1.0", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ExecuteListSelectedChanged()
        {
            selectedVDPItems = VDPItems.Where(vdp => vdp.IsSelected).ToList();
        }

        #endregion

        private void OpenPresetsOnStartup()
        {
            isFirstRun = false;
            presetManager.OpenPresetList(
                VDPItems
                .Select(vdp => vdp as VirtualDesktopPreset)
                .Where(vdp => vdp.IsOpenedOnSystemStart)
                .ToList());
        }
    }
}
