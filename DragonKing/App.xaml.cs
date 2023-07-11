using DragonKing.Log.Interface;
using DragonKing.Log.Service;
using DragonKing.View;
using DragonKing.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace DragonKing
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public new static App? Current => Application.Current as App;


        public App()
        {
            Services = ConfigureServices();

        }

        public IServiceProvider Services { get; private set; }

        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();
            services.AddSingleton<ILog, LogService>();
            services.AddSingleton<MainView>();
            services.AddSingleton<MainViewModel>();




            return services.BuildServiceProvider();
        }


        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var mainWindow = Services.GetService<MainView>();
            mainWindow.DataContext = Services.GetService<MainViewModel>();
            mainWindow!.Show();
        }
    }
}
