using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daily_News
{
    public class NewsArticle
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public DateTime DatePublished { get; set; }
        public List<NewsProvider> Provider { get; set; }
        public NewsImage Image { get; set; }

       
    }

}
