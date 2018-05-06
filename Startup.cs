using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace NoteBase
{
    public class Startup
    {
        public static Models.DbConnection dbConnection;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddDbContext<Models.DbModel>(options => options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));
            services.AddIdentity<Models.Users, Models.UserRole>( options => 
                                                                    {
                                                                        options.Password.RequiredLength = 6;
                                                                        options.Password.RequireNonAlphanumeric = false;
                                                                        options.Password.RequireLowercase = true;
                                                                        options.Password.RequireUppercase = true;
                                                                        options.Password.RequireDigit = true;
                                                                    }).AddEntityFrameworkStores<Models.DbModel>().AddDefaultTokenProviders();
			services.ConfigureApplicationCookie(options => options.LoginPath = "/Base/Authenticate");
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Base}/{action=Authenticate}/{id?}");
            });
        }
    }
}
