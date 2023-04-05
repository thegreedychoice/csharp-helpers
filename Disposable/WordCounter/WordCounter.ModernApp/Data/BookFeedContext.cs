using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordCounter.ModernApp.Data.Model;

namespace WordCounter.ModernApp.Data
{
    public class BookFeedContext : DbContext
    {
        public DbSet<BookFeed> BookFeeds { get; set; }
        public DbSet<BookLine> BookLines { get; set; }

        public BookFeedContext(DbContextOptions<BookFeedContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BookFeed>()
                .ToTable("BookFeed")
                .HasMany(x => x.BookLines)
                .WithOne(y => y.BookFeed)
                .HasForeignKey(y => y.BookFeedId);

            modelBuilder.Entity<BookLine>()
                .ToTable("BookLine");
        }
    }
}
