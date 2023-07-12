using DragonKing.Database.DbContext;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DragonKing.HostBuilders
{
    public static class AddDbContextHostBuilderExtensions
    {
        public static IHostBuilder AddDbContext(this IHostBuilder host)
        {
            host.ConfigureServices(services =>
            {
                services.AddSingleton<UserDbContext>();

            });

            return host;
        }
    }
}
