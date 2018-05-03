using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace NoteBase.Models
{
    public class DbModel : DbContext
    {
        public DbSet<Users> DbSetUsers { get; set; }
        public DbSet<Note> DbSetNotes { get; set; }
        public DbSet<Shares> DbSetShares { get; set; }

        public DbModel(DbContextOptions<DbModel> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Shares>().HasKey(sn => new { sn.Note_Id, sn.Owner_Id });
        }
    }
}
