using DragonKing.Database.DbContext;
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
