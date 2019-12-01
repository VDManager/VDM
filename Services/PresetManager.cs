using System.Collections.Generic;
using VDM.Data.Model;
using VDM.Services.Interfaces;

namespace VDM.Services
{
    public class PresetManager : IPresetManager
    {
        private readonly IVirtualDesktopService vdService;

        public PresetManager(IVirtualDesktopService vdService)
        {
            this.vdService = vdService;
        }

        public void OpenPresetList(IList<VirtualDesktopPreset> vdpList)
        {
            for (int i = 0; i < vdpList.Count; i++)
            {
                vdService.StartPresetOnDesktopAsync(vdpList[i], i).Wait();
            }
        }
    }
}
