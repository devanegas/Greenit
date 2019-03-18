using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Greenit.Models
{
    public class Channel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public string UserId { get; set; }
        public List<BlogPost> posts {get;set;}
        public List<User> AdminUsers { get; set;}
        public List<User> BlockedUsers { get; set; }
    }
    public class BlogPost
    {
        public int Id { get; set; }
        public string Slug { get; set; }
        public string Title { get; set; }
        public int ChannelId { get; set; }
        public string Body { get; set; }
        public DateTime Posted { get; set; }
        public string UserId { get; set; }
        public List<Comment> Comments { get; set; }
    }
    public class Comment
    {    
        public int Id { get; set; }
        public int PostId { get; set; }
        public string UserId { get; set; }
        public string Body { get; set; }
        public DateTime Posted { get; set; }
    }
    public class User
    {   
        public int Id { get; set;}
        public string Username { get; set;}
    }

}
