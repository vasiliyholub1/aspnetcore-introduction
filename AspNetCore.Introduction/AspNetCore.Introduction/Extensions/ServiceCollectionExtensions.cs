using System;
using AspNetCore.Introduction.Configuration;
using AspNetCore.Introduction.Interfaces;
using AspNetCore.Introduction.Repositories;
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

        public static IServiceCollection AddRepositoriesServices(this IServiceCollection services)
        {
            services.AddScoped<IDbProductRepository, DbProductRepository>();
            services.AddScoped<IDbCategoryRepository, DbCategoryRepository>();
            services.AddScoped<IDbRegionRepository, DbRegionRepository>();
            services.AddScoped<IDbSupplierRepository, DbSupplierRepository>();

            return services;
        }
    }
}