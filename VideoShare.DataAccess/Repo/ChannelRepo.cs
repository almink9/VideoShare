﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoShare.Core.Entities;
using VideoShare.Core.IRepo;
using VideoShare.DataAccess.Data;

namespace VideoShare.DataAccess.Repo
{
  public class ChannelRepo : BaseRepo<Channel>, IChannelRepo
  {
    private readonly Context _context;

    public ChannelRepo(Context context) : base(context)
    {
      _context = context;
    }

    public async Task<int> GetChannelIdByUserId(int userId)
    {
      return await _context.Channel.Where(x => x.AppUserId == userId).Select(x => x.Id).FirstOrDefaultAsync();
    }

  }
}
