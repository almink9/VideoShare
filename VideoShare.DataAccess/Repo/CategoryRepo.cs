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
  public class CategoryRepo : BaseRepo<Category>, ICategoryRepo
  {
    public CategoryRepo(Context context) : base(context)
    {

    }
  }
}
