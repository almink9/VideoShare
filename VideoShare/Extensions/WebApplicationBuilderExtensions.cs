using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VideoShare.DataAccess.Data;

namespace VideoShare.Extensions
{
  public static class WebApplicationBuilderExtensions
  {
    public static WebApplicationBuilder AddApplicationServices(this WebApplicationBuilder builder)
    {
      builder.Services.AddDbContext<Context>(options =>
      {
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
      });
      return builder;
    }
  }
}
