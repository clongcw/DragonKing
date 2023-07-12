using DragonKing.Database.Interface;
using DragonKing.Log.Interface;
using DragonKing.Log.Service;
using DragonKing.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonKing.HostBuilders
{
    public static class AddViewModelsHostBuilderExtensions
    {
        public static IHostBuilder AddViewModels(this IHostBuilder host)
        {
            host.ConfigureServices(services =>
            {
                services.AddSingleton<LoginViewModel>(s => new LoginViewModel(s.GetRequiredService<ILog>(), s.GetRequiredService<IUserService>()));
                services.AddSingleton<MainViewModel>(s => new MainViewModel(s.GetRequiredService<IUserService>(), s.GetRequiredService<IRoleService>()));

            });

            return host;
        }
    }
}
