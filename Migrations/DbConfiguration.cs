using System.Linq;
using NoteBase.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace NoteBase
{
	public class DbConfiguration : IdentityDbContext<Users, UserRole, int>
	{
		public DbConfiguration(DbContextOptions<DbConfiguration> opts) : base(opts) { }

		public static async Task Seed(IServiceProvider serviceProvider, IConfiguration config)
		{
			try
			{
				UserManager<Users> userManager = serviceProvider.GetRequiredService<UserManager<Users>>();

				Users user = new Users { UserName = "All", Id = -1 };

				if (await userManager.FindByNameAsync(user.UserName) == null)
				{
					IdentityResult res = await userManager.CreateAsync(user);
				}
			}
			catch (Exception e)
			{
				throw new DbUpdateException("Error while seeding the database", e);
			}
		}
	}
}

