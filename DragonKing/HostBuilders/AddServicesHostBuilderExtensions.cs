﻿using DragonKing.Database.Interface;
using DragonKing.Database.Service;
using DragonKing.Log.Interface;
using DragonKing.Log.Service;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DragonKing.HostBuilders
{
    public static class AddServicesHostBuilderExtensions
    {
        public static IHostBuilder AddServices(this IHostBuilder host)
        {
            host.ConfigureServices(services =>
            {
                services.AddSingleton<ILog, LogService>();
                services.AddSingleton<IRoleService, RoleService>();
                services.AddSingleton<IUserService, UserService>();
            });

            return host;
        }
    }
}
