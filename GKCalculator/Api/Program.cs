using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Core;

namespace Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var parsedData = ExcelParser.ParseExcel();

            GraphConverter.InitTpTable(parsedData);

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
