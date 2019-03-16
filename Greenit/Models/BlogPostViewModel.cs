using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Greenit.Models
{
    public class BlogPostViewModel
    {
        public BlogPostViewModel(IEnumerable<BlogPost> items)
        {
            Items = items ?? throw new ArgumentNullException(nameof(items));
        }
        public IEnumerable<BlogPost> Items { get; set; }
    }
}
