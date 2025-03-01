using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using VideoShare.Core.IRepo;

namespace VideoShare.Controllers
{
  public class CoreController : Controller
  {
    private IUnitOfWork _unitOfWork;
    protected IUnitOfWork UnitOfWork => _unitOfWork ??= HttpContext.RequestServices.GetService<IUnitOfWork>();
  }
}
