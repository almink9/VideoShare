using Microsoft.AspNetCore.Http;

namespace VideoShare.Services.IServices
{
  public interface IPhotoService
  {
    string UploadPhotoLocally(IFormFile photo, string oldPhotoUrl = "");
  }
}
