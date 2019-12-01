using System.Threading.Tasks;
using VDM.Data.Model;

namespace VDM.Services.Interfaces
{
    /// <summary>
    /// Provides functions to manage Windows virtual desktops.
    /// </summary>
    public interface IVirtualDesktopService
    {
        bool IsSupported { get; }
        void SetDesktopCount(int count, bool shouldRemove);

        /// <summary>
        /// Starts an app on the given VD
        /// </summary>
        /// <param name="preset">VDP to open</param>
        /// <param name="desktopNumber">0 based index  of the selected desktop</param>
        Task StartPresetOnDesktopAsync(VirtualDesktopPreset preset, int desktopNumber);
    }
}
