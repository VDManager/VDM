using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VDM.Data;
using VDM.Data.Model;
using VDM.Services.Interfaces;


namespace VDM.Services
{
    public class DatabaseService : IDatabaseService
    {
        private readonly AppDbContext dbContext;

        public DatabaseService(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<VirtualDesktopPreset>> GetAllPresets(CancellationToken token = default)
        {
            return await dbContext.VirtualDesktopPresets.AsNoTracking().Include(p => p.AttachedProcesses).AsNoTracking().ToListAsync(token);
        }

        public async Task<VirtualDesktopPreset> GetPreset(Guid id, CancellationToken token = default)
        {
            return await dbContext.VirtualDesktopPresets.AsNoTracking().Include(p => p.AttachedProcesses).AsNoTracking().FirstOrDefaultAsync(f => f.Id == id, token);
        }

        public async Task RemovePreset(VirtualDesktopPreset preset)
        {
            dbContext.Remove(preset);
            await dbContext.SaveChangesAsync();
        }

        public async Task AddPreset(VirtualDesktopPreset preset)
        {
            var e = dbContext.Add(preset);
            await dbContext.SaveChangesAsync();
            e.State = EntityState.Detached;
        }

        public async Task UpdatePreset(VirtualDesktopPreset preset)
        {
            var processIds = preset.AttachedProcesses.Select(p => p.Id);
            var dbPresetProcesses = dbContext.Processes.Where(p => p.PresetId == preset.Id && !processIds.Contains(p.Id));
            dbContext.Processes.RemoveRange(dbPresetProcesses);
            
            var ePreset = dbContext.VirtualDesktopPresets.Update(preset);
            await dbContext.SaveChangesAsync();

            ePreset.State = EntityState.Detached;
            foreach (var e in dbContext.ChangeTracker.Entries<Process>().Where(p=> processIds.Contains(p.Entity.Id)))
            {
                e.State = EntityState.Detached;
            }
        }
    }
}