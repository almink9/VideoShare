using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using VideoShare.Core.DTOs;
using VideoShare.Core.Pagination;
using VideoShare.Utility;
using VideoShare.ViewModels;
using VideoShare.ViewModels.Home;

namespace VideoShare.Controllers
{
  public class HomeController : CoreController
  {
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
      _logger = logger;
    }

    public async Task<IActionResult> Index(string page)
    {
      var toReturn = new Home_vm();
      if (page == null || page == "Home")
      {
        var allCategories = await UnitOfWork.CategoryRepo.GetAllAsync();

        var categoryList = allCategories.Select(category => new SelectListItem
        {
          Text = category.Name,
          Value = category.Id.ToString()
        }).ToList();

        categoryList.Insert(0, new SelectListItem
        {
          Text = "All",
          Value = "0",
          Selected = true
        });

        toReturn.CategoryDropdown = categoryList;
      }

      return View(toReturn);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
      return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    #region API Endpoints
    [Authorize(Roles = $"{SD.UserRole}")]
    [HttpGet]
    public async Task<IActionResult> GetVideosForHomeGrid(HomeParameters parameters)
    {
      var items = await UnitOfWork.VideoRepo.GetVideosForHomeGridAsync(parameters);
      var paginatedResults = new PaginatedResult<VideoForHomeGridDto>(items, items.TotalItemsCount, items.PageNumber, items.PageSize, items.TotalPages);

      return Json(new ApiResponse(200, result: paginatedResults));
    }
    #endregion
  }
}
