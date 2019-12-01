using System.Collections.Generic;
using VDM.Data.Model;

namespace VDM.Services.Interfaces
{
    public interface IPresetManager
    {
        void OpenPresetList(IList<VirtualDesktopPreset> vdpList);
    }
}
