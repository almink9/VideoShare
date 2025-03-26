using System;

namespace VideoShare.ViewModels.Video
{
  public class VideoWatch_vm
  {
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int ChannelId { get; set; }
    public string ChannelName { get; set; }
    public DateTime CreatedAt { get; set; }
    public int ViewersCount { get; set; }
  }
}
