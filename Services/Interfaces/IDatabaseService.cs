using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using VDM.Data.Model;



namespace VDM.Services.Interfaces
{
    /// <summary>
    /// Provides functions for persistence through a database.
    /// </summary>
    public interface IDatabaseService
    {
        Task<List<VirtualDesktopPreset>> GetAllPresets(CancellationToken token = default);
        Task<VirtualDesktopPreset> GetPreset(Guid id, CancellationToken token = default);
        Task RemovePreset(VirtualDesktopPreset preset);
        Task AddPreset(VirtualDesktopPreset preset);
        Task UpdatePreset(VirtualDesktopPreset preset);

    }
}