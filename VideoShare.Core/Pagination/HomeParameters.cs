using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoShare.Core.Pagination
{
  public class HomeParameters : BaseParameters
  {
    private string _searchBy;

    public string SearchBy
    {
      get => _searchBy;
      set => _searchBy = string.IsNullOrEmpty(value) ? "" : value.ToLower();
    }

    public int CategoryId { get; set; }
  }
}
