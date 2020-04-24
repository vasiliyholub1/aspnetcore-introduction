using System;
using AspNetCore.Introduction.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCore.Introduction.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddConfig(this IServiceCollection services, Action<MandatoryInfoConfiguration> options)
        {
            services.Configure(options);

            return services;
        }
    }
}