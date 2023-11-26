using Exam06.Models;
using Microsoft.EntityFrameworkCore;

namespace Exam06.Data
{
    public class LibraryDbContext : DbContext
    {
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<Author> Authors { get; set; }
        public virtual DbSet<Category> Categories { get; set; }

        public LibraryDbContext(DbContextOptions<LibraryDbContext> options) :
         base(options)
        {
            Database.Migrate();
        }
    }
}
