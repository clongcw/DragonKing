using DragonKing.HostBuilders;
using DragonKing.UI.Utils;
using DragonKing.View;
using DragonKing.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

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

            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MjYyMjk1N0AzMjMyMmUzMDJlMzBDN3ovUVFVWlE3cnd0QklDZ1hYUUtoSGlnbStnWE1NTytUUmpWNU5JTDlrPQ==");
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
