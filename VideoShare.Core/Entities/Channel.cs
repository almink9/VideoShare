using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoShare.Core.Entities
{
  public class Channel : BaseEntity
  {
    public string Name { get; set; }
    public string About { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public int AppUserId { get; set; }

    // Navigations
    public AppUser AppUser { get; set; }
    public ICollection<Video> Videos { get; set; }
    public ICollection<Subscribe> Subscribers { get; set; }
  }
}
