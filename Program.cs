using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;


namespace NoteBase
{
    public class Program
    {
        
        public static void Main(string[] args)
        {
            var WebHost = BuildWebHost(args);

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
