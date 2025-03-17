using Microsoft.EntityFrameworkCore;
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
  public class VideoRepo : BaseRepo<Video>, IVideoRepo
  {
    private readonly Context _context;

    public VideoRepo(Context context) : base(context)
    {
      _context = context;
    }
    
    public async Task<int> GetUserIdByVideoId(int videoId)
    {
      return await _context.Video
        .Where(x => x.Id == videoId)
        .Select(x => x.Channel.AppUserId)
        .FirstOrDefaultAsync();
    }
  }
}
