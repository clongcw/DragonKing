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
                services.AddSingleton(s => new LoginViewModel(s.GetRequiredService<ILog>(), s.GetRequiredService<IUserService>()));
                services.AddSingleton(s => new MainViewModel(s.GetRequiredService<IUserService>(), s.GetRequiredService<IRoleService>()));
                services.AddSingleton(s => new UserManagementViewModel(s.GetRequiredService<ILog>(), s.GetRequiredService<IUserService>(), s.GetRequiredService<IRoleService>()));
                services.AddSingleton<SettingsViewModel>();
                services.AddSingleton<ResultViewModel>();
                services.AddSingleton<TestViewModel>();
                services.AddSingleton<OfficeViewModel>();
                services.AddSingleton(s => new TaskViewModel(s.GetRequiredService<ILog>()));

            });

            return host;
        }
    }
}
