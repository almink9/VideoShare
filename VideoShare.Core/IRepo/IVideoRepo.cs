﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoShare.Core.Entities;

namespace VideoShare.Core.IRepo
{
  public interface IVideoRepo : IBaseRepo<Video>
  {
    Task<int> GetUserIdByVideoId(int videoId);
  }
}
