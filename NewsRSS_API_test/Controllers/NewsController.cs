using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsRSS.API.Data;
using System.ServiceModel.Syndication;
using System.Xml;

namespace NewsRSS_API_test.Controllers
{ 
    
    [Route("api/news")]
    [ApiController]
    public class NewsController : Controller
    {
        [HttpGet]
        public IActionResult AddNews()
        {
            List<NewsItem> news = new List<NewsItem>();
            using (var context = new RSSFeedDataContext())
            {
                foreach (RSSFeed feed in context.RSSFeeds)
                {
                    XmlReader reader = XmlReader.Create(feed.Url);
                    SyndicationFeed synfeed = SyndicationFeed.Load(reader);
                    reader.Close();
                    foreach (SyndicationItem item in synfeed.Items)
                    {
                        news.Add(new NewsItem(item.Title.Text, item.Summary.Text, false, item.PublishDate.UtcDateTime));
                    }
                }
                context.News.AddRange(news);
                context.SaveChanges();
            }
            return StatusCode(200);
        }

        [BasicAuth]
        [HttpGet("{date}")]
        public IActionResult GetUnreadNewsFromDate(DateTimeOffset date)
        {
            List<NewsItem> news = new List<NewsItem>();
            using (var context = new RSSFeedDataContext())
            {                
                news.AddRange(context.News.Where(x => DateTime.Compare(x.DatePosted.Date, date.UtcDateTime.Date.AddDays(1)) == 0 && !x.IsRead));
            }
            return new JsonResult(news);
        }

        [HttpPost]
        public IActionResult SetNewsAsRead()
        {
            using (var context = new RSSFeedDataContext())
            {
                foreach(NewsItem item in context.News)
                {
                    if(!item.IsRead)
                    {
                        item.IsRead = true;
                    }
                }
                context.SaveChanges();
            }
            return StatusCode(200);
        }
    }
}
