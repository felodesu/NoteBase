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
					var services = scope.ServiceProvider;
					DbConfiguration.Seed(services).Wait();
				}
				catch(System.Exception e)
				{
					throw new System.Exception("Error occured while seeding the database", e);
				}

			}

            WebHost.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
				.UseDefaultServiceProvider(opts => 
					opts.ValidateScopes = false)
                .Build();
    }
}
