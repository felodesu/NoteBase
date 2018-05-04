using System.Linq;
using NoteBase.Models;
using Microsoft.EntityFrameworkCore;

namespace NoteBase
{
    public static class Configuration
    {
        public static void Seed(this DbModel context)
        {
            try
            {
                var hasSeedUser = (from u in context.DbSetUsers
                                   where u.User_Id == -1
                                   select u).SingleOrDefault();

                if (hasSeedUser == null)
                {
                    context.Database.ExecuteSqlCommand("INSERT INTO DbSetUsers VALUES (-1, 'All', NULL)");
                }
            }
            catch { return;  }
        }
    }
}
