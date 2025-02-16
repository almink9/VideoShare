using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VideoShare.Utility;

namespace VideoShare.Controllers
{
  [Authorize(Roles = $"{SD.UserRole}")]
  public class ChannelController : Controller
  {
    public IActionResult Index()
    {
      return View();
    }
  }
}
