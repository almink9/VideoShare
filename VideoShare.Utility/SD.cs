﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Cryptography;

namespace VideoShare.Utility
{
  public static class SD
  {
    public const string AdminRole = "admin";
    public const string ModeratorRole = "moderator";
    public const string UserRole = "user";
    public static readonly List<string> Roles = new List<string> { AdminRole, UserRole, ModeratorRole };
    public const int MB = 1000000;
    public static readonly List<string> LocalIpAddresses = new List<string> { "127.0.0.1", "::1" };

    public static string IsActive(this IHtmlHelper html, string controller, string action, string cssClass = "active")
    {
      var routeData = html.ViewContext.RouteData;
      var routeAction = routeData.Values["action"]?.ToString();
      var routeController = routeData.Values["controller"]?.ToString();

      var returnActive = controller == routeController && action == routeAction;

      return returnActive ? cssClass : string.Empty;
    }

    public static string IsActivePage(this IHtmlHelper html, string page)
    {
      var currentPage = html.ViewContext.HttpContext.Request.Query["page"].ToString();
      var isPagematch = currentPage == page;

      // default Home page
      if (string.IsNullOrEmpty(currentPage) && page == "Home") return "active";

      return isPagematch ? "active" : string.Empty;
    }

    public static string GetRandomName()
    {
      var randomNumber = new byte[10];
      using var rng = RandomNumberGenerator.Create();
      rng.GetBytes(randomNumber);

      return Convert.ToBase64String(randomNumber);
    }

    public static string FormatView(int views)
    {
      if (views >= 1000000)
      {
        return (views / 1000000).ToString() + "M";
      }
      else if (views >= 1000)
      {
        return (views / 1000).ToString() + "K";
      }
      else
      {
        return views.ToString();
      }
    }

    public static string GetContentType(string fileExtension)
    {
      return fileExtension switch
      {
        ".mp4" => "video/mp4",
        ".mov" => "video/quicktime",
        ".avi" => "video/x-msvideo",
        ".wmv" => "video/x-ms-wmv",
        ".flv" => "video/x-flv",
        ".mkv" => "video/x-matroska",
        ".webm" => "video/webm",
        ".ogv" => "video/ogg",
        ".3gp" => "video/3gpp",
        ".3g2" => "video/3gpp2",
        _ => "video/mp4"
      };
    }

    public static string GetFileExtension(string contentType)
    {
      return contentType switch
      {
        "video/mp4" => ".mp4",
        "video/quickTime" => ".mov",
        "video/x-msvideo" => ".avi",
        "video/x-ms-wmv" => ".wmv",
        "video/x-flv" => ".flv",
        "video/x-matroska" => ".mkv",
        "video/webm" => ".webm",
        "video/ogg" => ".ogv",
        "video/3gpp" => ".3gp",
        "video/3gpp2" => ".3g2",
        _ => ".mp4"
      };
    }

    public static string TimeAgo(DateTime dateTime)
    {
      DateTime now = DateTime.UtcNow;
      TimeSpan timeSpan = now - dateTime;

      double totalSeconds = Math.Floor(timeSpan.TotalSeconds);
      double minutes = Math.Floor(timeSpan.TotalMinutes);
      double hours = Math.Floor(timeSpan.TotalHours);
      double days = Math.Floor(timeSpan.TotalDays);
      double months = Math.Floor(days / 30);
      double years = Math.Floor(days / 365);

      if (totalSeconds < 60)
      {
        return "Just now";
      }
      else if (minutes < 60)
      {
        return $"{minutes} minute{(minutes != 1 ? "s" : "")} ago";
      }
      else if (hours < 24)
      {
        return $"{hours} hour{(hours != 1 ? "s" : "")} ago";
      }
      else if (days < 30)
      {
        return $"{days} hour{(days != 1 ? "s" : "")} ago";
      }
      else if (days< 365)
      {
        return $"{months} month{(months != 1 ? "s" : "")} ago";
      }
      else
      {
        return $"{years} month{(years != 1 ? "s" : "")} ago";
      }
    }
  }
}
