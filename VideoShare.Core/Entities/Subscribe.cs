using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoShare.Core.Entities
{
  public class Subscribe
  {
    // PK (AppUserId, VideoId)
    // FK = AppUserId and FK = VideoId
    public int AppUserId { get; set; }
    public int ChannelId { get; set; }

    // Navigations
    public AppUser AppUser { get; set; }
    public Channel Channel { get; set; }
  }
}
