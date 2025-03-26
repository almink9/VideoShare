using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VideoShare.Core.IRepo;
using VideoShare.DataAccess.Data;

namespace VideoShare.Controllers
{
  public class CoreController : Controller
  {
    private IUnitOfWork _unitOfWork;
    private IConfiguration _configuration;
    private Context _context;
    protected IUnitOfWork UnitOfWork => _unitOfWork ??= HttpContext.RequestServices.GetService<IUnitOfWork>();
    protected IConfiguration Configuration => _configuration ??= HttpContext.RequestServices.GetService<IConfiguration>();
    protected Context Context => _context ??= HttpContext.RequestServices.GetService<Context>();
  }
}
