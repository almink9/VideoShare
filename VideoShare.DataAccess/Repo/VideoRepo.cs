using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoShare.Core.DTOs;
using VideoShare.Core.Entities;
using VideoShare.Core.IRepo;
using VideoShare.Core.Pagination;
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

    public async Task<PaginatedList<VideoForHomeGridDto>> GetVideosForHomeGridAsync(HomeParameters parameters)
    {
      var query = _context.Video
        .Select(x => new VideoForHomeGridDto
        {
          Id = x.Id,
          ThumbnailUrl = x.ThumnailUrl,
          Title = x.Title,
          Description = x.Description,
          CreatedAt = x.CreatedAt,
          ChannelName = x.Channel.Name,
          ChannelId = x.ChannelId,
          CategoryId = x.Category.Id,
          Views = x.Viewers.Count()
        })
        .AsQueryable();

      if (parameters.CategoryId > 0)
      {
        query = query.Where(x => x.CategoryId == parameters.CategoryId);
      }

      if (!string.IsNullOrEmpty(parameters.SearchBy))
      {
        query = query.Where(x => x.Title.ToLower().Contains(parameters.SearchBy) || x.Description.ToLower().Contains(parameters.SearchBy));
      }

      return await PaginatedList<VideoForHomeGridDto>.CreateAsync(query.AsNoTracking(), parameters.PageNumber, parameters.PageSize);
    }
  }
}
