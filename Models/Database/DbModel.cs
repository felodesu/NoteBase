using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NoteBase.Models
{
    public class DbModel : IdentityDbContext<Users, UserRole, int>
    {
        public DbSet<Users> DbSetUsers { get; set; }
        public DbSet<Note> DbSetNotes { get; set; }
        public DbSet<Shares> DbSetShares { get; set; }

        public DbModel(DbContextOptions<DbModel> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Shares>().HasKey(sn => new { sn.Note_Id, sn.UserId });
        }
    }
}
