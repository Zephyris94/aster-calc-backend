using Api.MappingProfiles;
using Core;
using Core.Services;
using Core.Settings;
using Core.Utility;
using Infrastructure;
using Infrastructure.Services;
using Infrastructure.Utility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Startup));

            services.AddOptions<DataSourceOptions>();

            services.Configure<DataSourceOptions>(Configuration.GetSection("DataSource"));

            services.AddAutoMapper(typeof(Startup));

            services.AddScoped<IPathFindingService, PathFindingService>();
            services.AddScoped<IGraphBuildingService, GraphBuildingService>();
            services.AddScoped<IPathFindingAlgorithm, DijkstraPathFindingAlgorithm>();
            services.AddSingleton<INodeCacheService, InMemoryNodeCacheService>();
            services.AddSingleton<IExcelParsing, ExcelParsing>();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(builder =>
                builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
