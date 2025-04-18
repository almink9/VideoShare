﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoShare.Core.IRepo
{
  public interface IUnitOfWork : IDisposable
  {
    IChannelRepo ChannelRepo { get; }
    ICategoryRepo CategoryRepo { get; }
    IVideoRepo VideoRepo { get; }
    IVideoFileRepo VideoFileRepo { get; }
    IVideoViewRepo VideoViewRepo { get; }

    Task<bool> CompleteAsync();
  }
}
