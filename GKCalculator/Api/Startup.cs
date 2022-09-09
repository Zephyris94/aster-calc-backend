using Api.ConfigurationExcensions;
using DataAccess;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System;

namespace Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            var nlogLoggerProvider = new NLogLoggerProvider();

            Logger = nlogLoggerProvider.CreateLogger(typeof(Startup).FullName);
        }

        public ILogger Logger { get; }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            try
            {
                services.AddAutoMapper(typeof(Startup));

                services.ConfigureOptions(Configuration, Logger);

                services.ConfigureDomainServices(Logger);

                services.ConfigureDataAccessRepositories(Logger);

                services.ConfigureDatabase(Configuration, Logger);

                services.ConfigureAzureServices(Configuration, Logger);

                services.AddCors(Configuration);

                services.AddControllers();

                services.AddSwaggerGen();
            }
            catch(Exception ex)
            {
                Logger.LogError(ex.Message, ex);
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ApplicationContext dataContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                dataContext.Database.Migrate();
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

            dataContext.VerifyEnums();
        }
    }
}
