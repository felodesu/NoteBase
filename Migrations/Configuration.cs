using System.Linq;
using NoteBase.Models;
using Microsoft.EntityFrameworkCore;

namespace NoteBase
{
    public static class Configuration
    {
        public static void Seed(this DbModel context)
        {
            Users seedUser = new Users { User_Id = 0, Name = "All" };
            try
            {
                var hasSeedUser = (from u in context.DbSetUsers
                                   where u.User_Id == 0
                                   select u).SingleOrDefault();

                if (hasSeedUser == null)
                {
                    context.Database.ExecuteSqlCommand("INSERT INTO DbSetUsers VALUES (0, 'All', NULL)");
                }
            }
            catch { return;  }
        }
    }
}
