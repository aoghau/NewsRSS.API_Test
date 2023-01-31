using Microsoft.AspNetCore.Mvc;
using System.Xml;
using System.ServiceModel.Syndication;
using NewsRSS.API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace NewsRSS_API_test.Controllers
{
    [Authorize]
    [Route("api/feeds")]
    [ApiController]
    public class RSSFeedController : Controller
    {
        [HttpGet]
        public IActionResult GetActiveFeeds()
        {
            List<RSSFeed> feeds = new List<RSSFeed>();
            using(RSSFeedDataContext context = new RSSFeedDataContext())
            {
                var active = context.RSSFeeds.Where(x => x.IsActive).ToList();
                feeds.AddRange(active);
            }
            return new JsonResult(feeds);
        }

        [HttpPost("{url}")]
        public IActionResult AddRSSFeed(string url)
        {
            string adress = url.Replace("%2F", "/");
            try
            {
                XmlReader read = XmlReader.Create(adress);
                read.Close();
            }
            catch 
            {
                return StatusCode(400);
            }
            XmlReader reader= XmlReader.Create(adress);
            SyndicationFeed feed = SyndicationFeed.Load(reader);
            reader.Close();
            string name = feed.Title.Text;
            RSSFeed rss = new RSSFeed(name, adress, true);
            using(var context = new RSSFeedDataContext())
            {
                context.RSSFeeds.Add(rss);
                context.SaveChanges();
            }
            return new ObjectResult(name + " added");
        }

        
    }
}
