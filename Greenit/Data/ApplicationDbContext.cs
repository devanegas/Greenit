using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Greenit.Models;

namespace Greenit.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Channel>()
                .HasMany<BlogPost>(b => b.posts)
                .WithOne();

            builder.Entity<BlogPost>()
                .HasMany<Comment>(b => b.Comments)
                .WithOne();
        }

        public DbSet<Channel> Channels { get; set; }
        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<Comment> comments { get; set; }
    }
}
