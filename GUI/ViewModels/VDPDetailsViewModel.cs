using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Win32;

using VDM.GUI.Helpers;
using VDM.GUI.ViewModels.Base;
using VDM.Data.Model;
using VDM.Services.Interfaces;


namespace VDM.GUI.ViewModels
{
    public class VDPDetailsViewModel : ViewModelBase
    {
        private readonly IDatabaseService dbService;

        public VDPDetailsViewModel(IDatabaseService dbService) : this(null, dbService) { }

        public VDPDetailsViewModel(VirtualDesktopPreset vdp, IDatabaseService dbService)
        {
           this.dbService = dbService;

            SaveCommand = new RelayCommand<object>(x => ExecuteSave(), c => CanExecuteSave);
            CancelCommand = new RelayCommand<object>(x => ExecuteCancel());
            BrowseIconCommand = new RelayCommand<object>(x => ExecuteBrowseIcon());
            DeleteIconCommand = new RelayCommand<object>(x => ExecuteDeleteIcon(), c => CanExecuteDeleteIcon);
            AddNewProcessCommand = new RelayCommand<object>(x => ExecuteAddNewProcess());
            RemoveSelectedProcessCommand = new RelayCommand<object>(x => ExecuteRemoveSelectedProcess(), c => CanExecuteRemoveSelectedProcess);
            RemoveAllEmptyProcessesCommand = new RelayCommand<object>(x => ExecuteRemoveAllEmptyProcessesCommand());

            Model = vdp ?? new VirtualDesktopPreset();

            Name = Model.Name;
            IsOpenedOnSystemStart = Model.IsOpenedOnSystemStart;

            if (Model.AttachedProcesses == null)
                AttachedProcesses = new ObservableCollection<Process>();
            else
                AttachedProcesses = new ObservableCollection<Process>(Model.AttachedProcesses);

            if (Model.Icon != null)
                IconSource = Converters.ByteArrayToImageSource(Model.Icon);
            else
                IconSource = null;
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand BrowseIconCommand { get; }
        public ICommand DeleteIconCommand { get; }
        public ICommand AddNewProcessCommand { get; }
        public ICommand RemoveSelectedProcessCommand { get; }
        public ICommand RemoveAllEmptyProcessesCommand { get; }



        private VirtualDesktopPreset model;
        public VirtualDesktopPreset Model
        {
            get => model;
            set => SetProperty(ref model, value);
        }

        private Process selectedProcess;
        public Process SelectedProcess
        {
            get => selectedProcess;
            set => SetProperty(ref selectedProcess, value);
        }

        public bool CanExecuteSave => !String.IsNullOrWhiteSpace(Name);
        private async void ExecuteSave()
        {
            Model.Name = Name;
            Model.IsOpenedOnSystemStart = IsOpenedOnSystemStart;
            Model.AttachedProcesses = AttachedProcesses;

            if (Model.Id == Guid.Empty)
                await dbService.AddPreset(Model);
            else
                await dbService.UpdatePreset(Model);

            Messenger.SignalReturnToList(true);
        }

        private void ExecuteCancel()
        {
            Messenger.SignalReturnToList(false);
        }

        private void ExecuteAddNewProcess()
        {
            AttachedProcesses.Add(new Process());
        }

        private void ExecuteBrowseIcon()
        {
            OpenFileDialog ofd = new OpenFileDialog()
            {
                AddExtension = true,
                CheckFileExists = true,
                CheckPathExists = true,
                Multiselect = false,
                ShowReadOnly = false,
                Filter = "Image files|*.jpg;*.jpeg;*.png;*.bmp;*.tiff",
                Title = "Select icon for VDP"
            };

            bool? success = ofd.ShowDialog();
            if (success.HasValue && success.Value)
            {
                Model.Icon = Converters.ImagePathToByteArray(ofd.FileName);
                IconSource = Converters.ByteArrayToImageSource(Model.Icon);
            }
        }

        public bool CanExecuteDeleteIcon => IconSource != null;
        private void ExecuteDeleteIcon()
        {
            IconSource = null;
            Model.Icon = null;
        }

        public bool CanExecuteRemoveSelectedProcess => SelectedProcess != null;
        private void ExecuteRemoveSelectedProcess()
        {
            AttachedProcesses.Remove(SelectedProcess);
            SelectedProcess = null;
        }

        private void ExecuteRemoveAllEmptyProcessesCommand()
        {
            List<Process> toRemove = new List<Process>();

            foreach (var process in AttachedProcesses)
            {
                if (String.IsNullOrWhiteSpace(process.Path))
                    toRemove.Add(process);
            }

            foreach (var process in toRemove)
            {
                AttachedProcesses.Remove(process);
            }
        }



        private string name;
        public string Name
        {
            get => name;
            set
            {
                SetProperty(ref name, value);
                NotifyPropertyChanged(nameof(CanExecuteSave));

                DoEasterEgg(value);
            }
        }

        private bool hasEasterEggRun = false;
        private void DoEasterEgg(string value)
        {
            if (value == "POD-624" && !hasEasterEggRun)
            {
                hasEasterEggRun = true;
                MessageBox.Show("kubectl delete pod", "kubectl delete pod", MessageBoxButton.YesNo, MessageBoxImage.Question);
            }
        }

        private bool isOpenedOnSystemStart;

        public bool IsOpenedOnSystemStart
        {
            get => isOpenedOnSystemStart;
            set => SetProperty(ref isOpenedOnSystemStart, value);
        }


        private ImageSource iconSource;
        public ImageSource IconSource
        {
            get => iconSource;
            set => SetProperty(ref iconSource, value);
        }

        private ObservableCollection<Process> attachedProcesses;

        public ObservableCollection<Process> AttachedProcesses
        {
            get => attachedProcesses;
            set => SetProperty(ref attachedProcesses, value);
        }
    }
}
