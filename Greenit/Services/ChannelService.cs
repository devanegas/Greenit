using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Greenit.Data;
using Greenit.Models;
using Microsoft.AspNetCore.Identity;


namespace Greenit.Services
{
    public class ChannelService : IChannelService
    {
        private readonly ApplicationDbContext _context;

        public ChannelService(ApplicationDbContext DbContext)
        {
            _context = DbContext ?? throw new ArgumentNullException(nameof(DbContext));
        }

        public async Task<Channel[]> GetChannelsAsync()
        {
            return await _context.Channels
                .Include(c => c.posts)
                .ToArrayAsync();

        }

        public async Task<bool> AddChannelAsync(Channel newChannel)
        {
            _context.Channels.Add(newChannel);
            var saveResult = await _context.SaveChangesAsync();
            return (saveResult == 1);
        }

    }
}
