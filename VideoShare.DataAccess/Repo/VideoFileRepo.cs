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
  public class VideoFileRepo : BaseRepo<VideoFile>, IVideoFileRepo
  {
    public VideoFileRepo(Context context) : base(context)
    {

    }
  }
}
