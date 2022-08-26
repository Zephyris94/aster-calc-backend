using Azure.Core;
using Azure.Identity;
using Core;
using Core.Services;
using Core.Settings;
using Core.Utility;
using DataAccess;
using DataAccess.Repositories;
using Infrastructure;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Infrastructure.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Api.ConfigurationExcensions
{
    public static class ConfigurationExtensions
    {
        public static void ConfigureDomainServices(this IServiceCollection services)
        {
            services.AddScoped<IPathFindingService, PathFindingService>();
            services.AddScoped<IGraphBuildingService, GraphBuildingService>();
            services.AddScoped<IRouteProviderService, RouteProviderService>();
            services.AddScoped<IPathFindingAlgorithm, DijkstraPathFindingAlgorithm>();
            services.AddScoped<IDataMigrationService, ExcelDataMigrationService>();
            services.AddScoped<IBlobStorageService, BlobStorageService>();

            services.AddSingleton<INodeCacheService, InMemoryNodeCacheService>();
            services.AddSingleton<IExcelParsing, ExcelParsing>();
        }

        public static void ConfigureDataAccessRepositories(this IServiceCollection services)
        {
            services.AddScoped<IRouteRepository, RouteRepository>();
            services.AddScoped<ICalculationRepository, CalculationRepository>();
        }

        public static void ConfigureAzureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAzureClients(builder =>
            {
                builder.AddBlobServiceClient(configuration.GetConnectionString("AzureBlobStorageString"));
            });
        }

        public static void ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("AzureSqlConnection"),
                    b => b.MigrationsAssembly(typeof(ApplicationContext).Assembly.FullName)));
        }

        public static void ConfigureOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<DataSourceOptions>();
            services.Configure<DataSourceOptions>(configuration.GetSection("DataSource"));
        }

        public static void AddCors(this IServiceCollection services, IConfiguration configuration)
        {
            var policyName = configuration.GetValue<string>("CORS:PolicyName");
            var allowedHosts = configuration.GetSection("CORS:AllowedHosts").Get<string[]>();

            services.AddCors(options =>
            {
                options.AddPolicy(name: policyName,
                    builder =>
                    {
                        builder.WithOrigins(allowedHosts)
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });
        }
    }
}
