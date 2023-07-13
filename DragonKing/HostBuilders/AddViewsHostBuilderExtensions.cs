using DragonKing.View;
using DragonKing.View.UserManagement;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DragonKing.HostBuilders
{
    public static class AddViewsHostBuilderExtensions
    {
        public static IHostBuilder AddViews(this IHostBuilder host)
        {
            host.ConfigureServices(services =>
            {
                services.AddSingleton<MainView>();
                services.AddSingleton<LoginView>();
                services.AddSingleton<UserManagementView>();
            });

            return host;
        }
    }
}
