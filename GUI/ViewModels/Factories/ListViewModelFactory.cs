using System.Collections.Generic;
using VDM.Data.Model;
using VDM.Services.Interfaces;

namespace VDM.GUI.ViewModels.Factories
{
    public class ListViewModelFactory
    {
        private readonly IDatabaseService dbService;
        private readonly IPresetManager presetManager;

        public ListViewModelFactory(IDatabaseService dbService, IPresetManager presetManager)
        {
            this.dbService = dbService;
            this.presetManager = presetManager;
        }

        public ListViewModel Build(List<VirtualDesktopPreset> vdpList = null)
        {
            return new ListViewModel(vdpList, dbService, presetManager);
        }
    }
}
