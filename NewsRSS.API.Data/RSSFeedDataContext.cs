using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsRSS.API.Data
{
    public class RSSFeedDataContext : DbContext
    {
        public RSSFeedDataContext(DbContextOptions<RSSFeedDataContext> options) : base(options) 
        {
            
        }

        public RSSFeedDataContext() : base()
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("Server=localhost;Database=rssdb;Port=5432;User Id=postgres;Password=l'horizon");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseSerialColumns();            
        }

        public DbSet<RSSFeed> RSSFeeds { get; set;} 
        public DbSet<NewsItem> News { get; set;}
    }
}
