using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VideoShare.ViewModels.Video
{
  public class VideoAddEdit_vm
  {
    public int Id { get; set; }
    [Required]
    public string Title { get; set; }
    [Required]
    public string Description { get; set; }
    [Display(Name = "Upload thumbnail here")]
    public IFormFile ImageUpload { get; set; }
    [Display(Name = "Upload your video here")]
    public IFormFile VideoUpload { get; set; }
    [Display(Name = "Choose the category for your video")]
    [Required(ErrorMessage = "Please select a category")]
    public int CategoryId { get; set; }
    public IEnumerable<SelectListItem> CategoryDropdown { get; set; }
    public string ImageContentTypes { get; set; }
    public string VideoContentTypes { get; set; }
    public string ImageUrl { get; set; }
  }
}
