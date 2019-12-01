using VDM.Data.Model;
using VDM.Services.Interfaces;

namespace VDM.GUI.ViewModels.Factories
{
    public class VDPDetailsViewModelFactory
    {
        private readonly IDatabaseService dbService;

        public VDPDetailsViewModelFactory(IDatabaseService dbService)
        {
            this.dbService = dbService;
        }

        public VDPDetailsViewModel Build(VirtualDesktopPreset vdp = null)
        {
            return new VDPDetailsViewModel(vdp, dbService);
        }
    }
}
