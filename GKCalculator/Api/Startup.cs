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
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
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

            services.AddScoped<IRouteRepository, RouteRepository>();
            services.AddScoped<ICalculationRepository, CalculationRepository>();

            services.AddScoped<IPathFindingService, PathFindingService>();
            services.AddScoped<IGraphBuildingService, GraphBuildingService>();
            services.AddScoped<IRouteProviderService, RouteProviderService>();
            services.AddScoped<IPathFindingAlgorithm, DijkstraPathFindingAlgorithm>();
            services.AddScoped<IDataMigrationService, ExcelDataMigrationService>();

            services.AddSingleton<INodeCacheService, InMemoryNodeCacheService>();
            services.AddSingleton<IExcelParsing, ExcelParsing>();

            services.AddDbContext<ApplicationContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(ApplicationContext).Assembly.FullName)));

            services.AddCors(options =>
            {
                options.AddPolicy(name: "AllowOrigin",
                    builder =>
                    {
                        builder.WithOrigins("https://localhost:44351", "http://localhost:4200", "https://aster-calc.vercel.app")
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });

            services.AddControllers();

            services.AddSwaggerGen();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ApplicationContext dataContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("AllowOrigin");

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            dataContext.Database.Migrate();
            dataContext.VerifyEnums();
        }
    }
}
