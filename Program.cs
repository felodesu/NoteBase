using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace NoteBase
{
    public class Program
    {
        
        public static void Main(string[] args)
        {
            var WebHost = BuildWebHost(args);

            using (var scope = WebHost.Services.CreateScope())
            {
                try
                {
                    scope.ServiceProvider.GetService<Models.DbModel>().Seed();
                }
                catch (Exception ex)
                {
                    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }

            WebHost.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
