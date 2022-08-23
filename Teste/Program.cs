using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace Teste
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            var builder = new HostBuilder()
                            .ConfigureServices((hostContext, services) =>
                            {
                                services.AddScoped<Form1>();
                                services.AddLogging(option =>
                                {
                                    option.SetMinimumLevel(LogLevel.Information);
                                    option.AddNLog("nlog.config");
                                });
                            });
            ApplicationConfiguration.Initialize();
            var host = builder.Build();
            using (var serviceScope = host.Services.CreateScope())
            {
                var services = serviceScope.ServiceProvider;
                try
                {
                    var form1 = services.GetRequiredService<Form1>();                    
                    Application.Run(form1);
                }
                catch (Exception)
                {
                    throw;
                }
            }

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            //ApplicationConfiguration.Initialize();
            //Application.Run(new Form1());  
        }
    }
}