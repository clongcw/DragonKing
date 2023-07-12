using DragonKing.HostBuilders;
using DragonKing.Log.Interface;
using DragonKing.Log.Service;
using DragonKing.View;
using DragonKing.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog.Core;
using SqlSugar;
using System;
using System.Collections;
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

        public readonly IHost _host;


        public App()
        {
            _host = CreateHostBuilder().Build();
        }

        public static IHostBuilder CreateHostBuilder(string[] args = null)
        {
            return Host.CreateDefaultBuilder(args)
                .AddDbContext()
                .AddServices()
                .AddViewModels()
                .AddViews();
        }



        private void Application_Startup(object sender, StartupEventArgs e)
        {
            _host.Start();

            var loginView = _host.Services.GetRequiredService<LoginView>();
            loginView.DataContext = _host.Services.GetRequiredService<LoginViewModel>();
            loginView!.Show();
        }
    }
}
