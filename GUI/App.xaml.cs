using System;
using System.IO;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using VDM.Data;
using VDM.GUI.Helpers;
using VDM.GUI.Views;
using VDM.GUI.ViewModels;
using VDM.GUI.ViewModels.Factories;
using VDM.Services.Interfaces;
using VDM.Services;

namespace VDM.GUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IServiceProvider serviceProvider;

        public App()
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            serviceProvider = serviceCollection.BuildServiceProvider();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>();
            services.AddScoped<IDatabaseService, DatabaseService>();
            services.AddSingleton<IVirtualDesktopService, VirtualDesktopService>();
            services.AddSingleton<IPresetManager, PresetManager>();
            
            services.AddTransient<ListViewModelFactory>();
            services.AddTransient<VDPDetailsViewModelFactory>();
            services.AddTransient<MainWindowViewModel>();

            services.AddScoped<MainWindow>();
        }

        private async void OnStartup(object sender, StartupEventArgs e) // Entry point before GUI loads
        {
            EnsureStartupShortcutExists();

            using (var scope = serviceProvider.CreateScope())
            {
                var ctx = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                await ctx.Database.EnsureCreatedAsync();
            }
           
            var mainWindow = serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private void EnsureStartupShortcutExists()
        {
            try
            {
                string startupShortcutFullPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.Startup),
                    "VDM.Ink");

                if (!File.Exists(startupShortcutFullPath))
                    CreateShortcut();
            }
            catch (Exception)
            {
                ExceptionHandler.ShowExceptionNotification("Startup shortcut cannot be created",
                    "Cannot create startup shortcut, so opening on system start feature will not work.", null);
            }
        }

        private void CreateShortcut()
        {
            var shell = new IWshRuntimeLibrary.WshShell();

            object shDesktop = "Startup";
            string shortcutAddress = (string)shell.SpecialFolders.Item(ref shDesktop) + @"\VDM.lnk";

            var shortcut = (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(shortcutAddress);
            shortcut.Description = "Startup shortcut for VDM";
            shortcut.TargetPath = Path.Combine(Directory.GetCurrentDirectory(), "VDM.exe");
            shortcut.WorkingDirectory = Directory.GetCurrentDirectory();
            shortcut.Save();
        }
    }
}
