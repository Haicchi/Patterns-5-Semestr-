using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Lab1.Model;

namespace Lab1
{
    public class AppDbContext:DbContext
    {
        public DbSet<LibraryItem> LibraryItems{ get; set; }
        public DbSet<Book> Books{  get; set; }

        public DbSet<Newspaper> Newspapers { get; set; }
        public DbSet<NewspaperColumn> NewspaperColumns { get; set; }
        public DbSet<Almanac> Almanacs { get; set; }

        public DbSet<AlmanacBook> AlmanacBooks {  get; set; }

        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=library_db;Username=postgres;Password=197536");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<LibraryItem>().ToTable("LibraryItems");
            modelBuilder.Entity<Book>().ToTable("Books");
            modelBuilder.Entity<Newspaper>().ToTable("Newspapers");
            modelBuilder.Entity<Almanac>().ToTable("Almanacs");
            modelBuilder.Entity<NewspaperColumn>().ToTable("NewspaperColumns");
            modelBuilder.Entity<AlmanacBook>().ToTable("AlmanacBooks");
            modelBuilder.Entity<Newspaper>().Navigation(n => n.Columns).AutoInclude();
            modelBuilder.Entity<Almanac>().Navigation(a => a.Books).AutoInclude();
        }

    }
}
