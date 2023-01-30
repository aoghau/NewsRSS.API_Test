using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsRSS.API.Data
{
    public class NewsItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public bool IsRead { get; set; } = false;
        public DateTimeOffset DatePosted { get; set; }
        public NewsItem(string title, string summary, bool isRead, DateTimeOffset datePosted)
        {            
            Title = title;
            Summary = summary;
            IsRead = isRead;
            DatePosted = datePosted;
        }
    }
}
