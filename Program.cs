using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
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
                catch (Exception e)
                {
                    throw new Exception("Error occured while seeding the database", e);
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
