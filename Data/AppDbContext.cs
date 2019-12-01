using System.Reflection;
using Microsoft.EntityFrameworkCore;

using VDM.Data.Model;



namespace VDM.Data
{
    public class AppDbContext: DbContext
    {
        public DbSet<Process> Processes { get; set; }
        public DbSet<VirtualDesktopPreset> VirtualDesktopPresets { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(@"Data Source=elfszoft.db;", options =>
            {
                options.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
            });

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Map table names
            modelBuilder.Entity<Process>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Path).IsRequired();
            });
            modelBuilder.Entity<VirtualDesktopPreset>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.HasMany(e => e.AttachedProcesses)
                    .WithOne(p => p.Preset).HasForeignKey(p => p.PresetId)
                    .IsRequired();
            });


            base.OnModelCreating(modelBuilder);
        }
    }

}
