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
        /// <summary>
        /// Method gets all active RSS feeds
        /// </summary>
        /// <returns>A JSON with all active feeds, or an empty one if there are none</returns>
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

        /// <summary>
        /// Gets info abouth an RSS feed using given url, and adds the given feed into DB
        /// </summary>
        /// <param name="url">a URL</param>
        /// <returns>Bad request if there is no resource with that url, and "name added" if the resource exists</returns>
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
