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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseSerialColumns();
        }

        public DbSet<RSSFeed> RSSFeeds { get; set;}
    }
}
