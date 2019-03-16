using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Greenit.Models;

namespace Greenit.Services
{
    public interface IChannelService
    {
        Task<Channel[]> GetChannelsAsync();
        Task<bool> AddChannelAsync(Channel newChannel);
    }
}
