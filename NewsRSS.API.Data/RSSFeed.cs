using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsRSS.API.Data
{
    public class RSSFeed
    {
        public int Id {get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public bool IsActive { get; set; } = true;
        public RSSFeed(string name, string url, bool isActive)
        {            
            Name = name;
            Url = url;
            IsActive = isActive;
        }
    }
}
