﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoShare.Core.Entities
{
  public class AppUser : IdentityUser<int>
  {
    [Required]
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    // Navigations
    public Channel Channel { get; set; }
    public ICollection<Comment> Comments { get; set; }
    public ICollection<Subscribe> Subscriptions { get; set; }
    public ICollection<LikeDislike> LikeDislikes { get; set; }
    public ICollection<VideoView> Histories { get; set; }
  }
}
