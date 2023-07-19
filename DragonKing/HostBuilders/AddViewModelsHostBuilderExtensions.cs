using DragonKing.Database.Interface;
using DragonKing.Log.Interface;
using DragonKing.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
                services.AddSingleton<UserManagementViewModel>(s => new UserManagementViewModel(s.GetRequiredService<ILog>(), s.GetRequiredService<IUserService>(), s.GetRequiredService<IRoleService>()));
                services.AddSingleton<SettingsViewModel>();
                services.AddSingleton<ResultViewModel>();

            });

            return host;
        }
    }
}
