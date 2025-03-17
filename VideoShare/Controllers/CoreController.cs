using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VideoShare.Core.IRepo;

namespace VideoShare.Controllers
{
  public class CoreController : Controller
  {
    private IUnitOfWork _unitOfWork;
    private IConfiguration _configuration;
    protected IUnitOfWork UnitOfWork => _unitOfWork ??= HttpContext.RequestServices.GetService<IUnitOfWork>();
    protected IConfiguration Configuration => _configuration ??= HttpContext.RequestServices.GetService<IConfiguration>();
  }
}
