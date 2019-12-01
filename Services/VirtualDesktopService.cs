using System;
using System.Diagnostics;
using System.Threading.Tasks;
using WindowsDesktop;
using VDM.Data.Model;
using VDM.Services.Interfaces;
using Process = System.Diagnostics.Process;
using System.Linq;

namespace VDM.Services
{
    /// <summary>
    /// Provides functions to manage Windows virtual desktops.
    /// </summary>
    public class VirtualDesktopService : IVirtualDesktopService
    {
        private VirtualDesktop[] VirtualDesktops => VirtualDesktop.GetDesktops();

        private int VDCount => VirtualDesktops.Length;
        public bool IsSupported => VirtualDesktop.IsSupported;

        public void SetDesktopCount(int count, bool shouldRemove)
        {
            if (count < 0)
                throw new ArgumentException("Count must be > 0", nameof(count));

            if (count > 64)
                throw new ArgumentOutOfRangeException(nameof(count), "Prolly shouldn't create more than 64 VDs.");

            if (shouldRemove && VDCount > count)
                RemoveDesktops(VDCount - count);
            else if (VDCount < count)
                AddDesktops(count - VDCount);
        }

        public Task StartPresetOnDesktopAsync(VirtualDesktopPreset preset, int desktopNumber)
        {
            if (desktopNumber <= VDCount)
                SetDesktopCount(desktopNumber + 1, false);
            var targetDesktop = VirtualDesktops[desktopNumber];
            targetDesktop.Switch();
            var openJobs = preset.AttachedProcesses.Select(p => StartProcessAsync(p.Path, p.CommandLineArgs)).ToList();
            return Task.WhenAll(openJobs);
        }

        private Task StartProcessAsync(string appPath, string appArgs = null, bool waitForOpen = true, uint timeout = 4096)
        {
            return Task.Run(async () =>
            {
                var startInfo = new ProcessStartInfo(appPath, appArgs)
                {
                    WindowStyle = ProcessWindowStyle.Normal
                };
                var proc = Process.Start(startInfo);
                if (!waitForOpen) return;
                for (var backOff = 1; proc?.MainWindowHandle.ToInt64() == 0 && backOff <= timeout; backOff <<= 1)
                {
                    await Task.Delay(backOff);
                    proc.Refresh();
                }
            });
        }

        private void AddDesktops(int c)
        {
            for (var i = 0; i < c; i++)
            {
                VirtualDesktop.Create();
            }
        }

        private void RemoveDesktops(int c)
        {
            var toRemove = VirtualDesktops[^c..];
            foreach (var virtualDesktop in toRemove)
            {
                virtualDesktop.Remove();
            }
        }
    }
}