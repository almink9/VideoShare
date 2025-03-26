using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoShare.Core.Pagination
{
  public class PaginatedList<T> : List<T>
  {
    public PaginatedList(IEnumerable<T> items, int totalItemsCount, int pageNumber, int pageSize, int totalPages)
    {
      PageNumber = pageNumber;
      PageSize = pageSize;
      TotalPages = totalPages;
      TotalItemsCount = totalItemsCount;
      AddRange(items);
    }

    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public int TotalItemsCount { get; set; }

    #region Static Methods
    public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
    {
      var totalItemsCount = await source.CountAsync();
      var totalPages = (int)Math.Ceiling(totalItemsCount / (double)pageSize);
      if (pageNumber < 1)
      {
        pageNumber = 1;
      }
      else if (pageNumber > totalPages)
      {
        pageNumber = totalPages;
      }
      var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
      return new PaginatedList<T>(items, totalItemsCount, pageNumber, pageSize, totalPages);
    }
    #endregion
  }
}
