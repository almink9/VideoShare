using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VideoShare.Core.DTOs;
using VideoShare.Core.Entities;
using VideoShare.Core.Pagination;
using VideoShare.Extensions;
using VideoShare.Services.IServices;
using VideoShare.Utility;
using VideoShare.ViewModels;
using VideoShare.ViewModels.Video;

namespace VideoShare.Controllers
{
  [Authorize(Roles = $"{SD.UserRole}")]
  public class VideoController : CoreController
  {
    private readonly IPhotoService _photoService;

    public VideoController(IPhotoService photoService)
    {
      _photoService = photoService;
    }

    public async Task<IActionResult> Watch(int id)
    {
      var toReturn = await GetVideoWatch_vmWithProjections(id);

      if (toReturn != null)
      {
        var userIpAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
        await UnitOfWork.VideoViewRepo.HandleVideoViewAsync(User.GetUserId(), id, userIpAddress);
        await UnitOfWork.CompleteAsync();

        return View(toReturn);
      }

      TempData["notification"] = "false;Not Found;Requested video was not found";
      return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> CreateEditVideo(int id)
    {
      if (!await UnitOfWork.ChannelRepo.AnyAsync(x => x.AppUserId == User.GetUserId()))
      {
        TempData["notification"] = "false;Not Found;No channel associated with your account. Please create a channel first.";
        return RedirectToAction("Index", "Channel");
      }

      var toReturn = new VideoAddEdit_vm();
      toReturn.ImageContentTypes = string.Join(",", AcceptableContentTypes("image"));
      toReturn.VideoContentTypes = string.Join(",", AcceptableContentTypes("video"));

      if (id > 0)
      {
        // edit
        var userId = await UnitOfWork.VideoRepo.GetUserIdByVideoId(id);
        if (!userId.Equals(User.GetUserId()))
        {
          TempData["notification"] = "false;Not Found;Video not found";
          return RedirectToAction("Index", "Channel");
        }
        var fetchedVideo = await UnitOfWork.VideoRepo.GetByIdAsync(id);
        if (fetchedVideo == null)
        {
          TempData["notification"] = "false;Not Found;Video not found";
          return RedirectToAction("Index", "Channel");
        }
        toReturn.Id = fetchedVideo.Id;
        toReturn.Title = fetchedVideo.Title;
        toReturn.Description = fetchedVideo.Description;
        toReturn.CategoryId = fetchedVideo.CategoryId;
        toReturn.ImageUrl = fetchedVideo.ThumnailUrl;
      }

      toReturn.CategoryDropdown = await GetCategoryDropdownAsync();

      return View(toReturn);
    }

    [HttpPost]
    public async Task<IActionResult> CreateEditVideo(VideoAddEdit_vm model)
    {
      if (ModelState.IsValid)
      {
        bool proceed = true;

        if (model.Id == 0)
        {
          if (model.ImageUpload == null)
          {
            ModelState.AddModelError("ImageUpload", "Please upload thumbnail");
            proceed = false;
          }

          if (proceed && model.VideoUpload == null)
          {
            ModelState.AddModelError("VideoUpload", "Please upload your video");
            proceed = false;
          }
        }

        if (model.ImageUpload != null)
        {
          if (proceed && !IsAcceptableContentType("image", model.ImageUpload.ContentType))
          {
            ModelState.AddModelError("ImageUpload", string.Format("Invalid content type. It must be one of the following: {0}",
              string.Join(", ", AcceptableContentTypes("image"))));
            proceed = false;
          }

          if (proceed && model.ImageUpload.Length > int.Parse(Configuration["FileUpload:ImageMaxSizeInMB"]) * SD.MB)
          {
            ModelState.AddModelError("ImageUpload", string.Format("The uploaded file should not exceed {0} MB",
              int.Parse(Configuration["FileUpload:ImageMaxSizeInMB"])));
            proceed = false;
          }
        }

        if (model.VideoUpload != null)
        {
          if (proceed && !IsAcceptableContentType("video", model.VideoUpload.ContentType))
          {
            ModelState.AddModelError("VideoUpload", string.Format("Invalid content type. It must be one of the following: {0}",
              string.Join(", ", AcceptableContentTypes("video"))));
            proceed = false;
          }

          if (proceed && model.VideoUpload.Length > int.Parse(Configuration["FileUpload:VideoMaxSizeInMB"]) * SD.MB)
          {
            ModelState.AddModelError("VideoUpload", string.Format("The uploaded file should not exceed {0} MB",
              int.Parse(Configuration["FileUpload:VideoMaxSizeInMB"])));
            proceed = false;
          }
        }

        if (proceed)
        {
          string title = "";
          string message = "";

          if (model.Id == 0)
          {
            // create
            var videoToAdd = new Video()
            {
              Title = model.Title,
              Description = model.Description,
              VideoFile = new VideoFile
              {
                ContentType = model.VideoUpload.ContentType,
                Contents = GetContentsAsync(model.VideoUpload).GetAwaiter().GetResult(),
                Extension = SD.GetFileExtension(model.VideoUpload.ContentType)
              },
              CategoryId = model.CategoryId,
              ChannelId = UnitOfWork.ChannelRepo.GetChannelIdByUserId(User.GetUserId()).GetAwaiter().GetResult(),
              ThumnailUrl = _photoService.UploadPhotoLocally(model.ImageUpload)
            };

            UnitOfWork.VideoRepo.Add(videoToAdd);

            title = "Created";
            message = "Video created successfully";
          }
          else
          {
            // for update
            var fetchedVideo = await UnitOfWork.VideoRepo.GetByIdAsync(model.Id);
            if (fetchedVideo == null)
            {
              TempData["notification"] = "false;Not Found;Video not found";
              return RedirectToAction("Index", "Channel");
            }

            fetchedVideo.Title = model.Title;
            fetchedVideo.Description = model.Description;
            fetchedVideo.CategoryId = model.CategoryId; 

            if (model.ImageUpload != null)
            {
              fetchedVideo.ThumnailUrl = _photoService.UploadPhotoLocally(model.ImageUpload, fetchedVideo.ThumnailUrl);
            }

            title = "Edited";
            message = "Video has been updated";
          }

          TempData["notification"] = $"true;{title};{message}"; 
          await UnitOfWork.CompleteAsync();
          return RedirectToAction("Index", "Channel");
        }
      }

      model.CategoryDropdown = await GetCategoryDropdownAsync();

      return View(model);
    }

    public async Task<IActionResult> GetVideoFile(int videoId)
    {
      var fetchedVideoFile = await UnitOfWork.VideoFileRepo.GetFirstOrDefaultAsync(x => x.VideoId == videoId);
      if (fetchedVideoFile != null)
      {
        return File(fetchedVideoFile.Contents, fetchedVideoFile.ContentType);
      }

      TempData["notification"] = "false;Not Found;Requested video was not found";
      return RedirectToAction("Index", "Home");
    }

    #region API endpoints
    //[HttpGet]
    //public async Task<IActionResult> GetVideosForChannelGrid(BaseParameters parameters) 
    //{
    //  var userChannelId = await UnitOfWork.ChannelRepo.GetChannelIdByUserId(User.GetUserId());
    //  var videosForGrid = await UnitOfWork.VideoRepo.GetVi
    //}
    #endregion

    #region Private Methods
    public async Task<IEnumerable<SelectListItem>> GetCategoryDropdownAsync()
    {
      var allCategories = await UnitOfWork.CategoryRepo.GetAllAsync();
      return allCategories.Select(category => new SelectListItem()
      {
        Text = category.Name,
        Value = category.Id.ToString()
      });
    }

    private string[] AcceptableContentTypes(string type)
    {
      if (type.Equals("image"))
      {
        return Configuration.GetSection("FileUpload:ImageContentTypes").Get<string[]>();
      }
      else
      {
        return Configuration.GetSection("FileUpload:VideoContentTypes").Get<string[]>();
      }
    }

    private bool IsAcceptableContentType(string type, string contentType)
    {
      var allowedContentTypes = AcceptableContentTypes(type);
      foreach (var allowedContentType in allowedContentTypes)
      {
        if (contentType.ToLower().Equals(allowedContentType.ToLower()))
        {
          return true;
        }
      }

      return false;
    }

    private async Task<byte[]> GetContentsAsync(IFormFile file)
    {
      byte[] contents;
      using var memoryStream = new MemoryStream();
      await file.CopyToAsync(memoryStream);
      contents = memoryStream.ToArray();
      return contents;
    }

    private async Task<VideoWatch_vm> GetVideoWatch_vmWithProjections(int id)
    {
      int userId = User.GetUserId();
      var toReturn = await Context.Video
        .Where(x => x.Id == id)
        .Select(x => new VideoWatch_vm
        {
          Id = x.Id,
          Title = x.Title,
          Description = x.Description,
          ChannelId = x.ChannelId,
          ChannelName = x.Channel.Name,
          CreatedAt = x.CreatedAt,
          ViewersCount = x.Viewers.Count
        })
        .FirstOrDefaultAsync();

      return toReturn;
    }
    #endregion
  }
}
