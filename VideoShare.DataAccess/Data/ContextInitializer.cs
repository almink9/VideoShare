using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoShare.Core.Entities;
using VideoShare.Utility;

namespace VideoShare.DataAccess.Data
{
  public static class ContextInitializer
  {
    public static async Task InitializeAsync(Context context,
      UserManager<AppUser> userManager,
      RoleManager<AppRole> roleManager)
    {
      if (context.Database.GetPendingMigrations().Count() > 0)
      {
        context.Database.Migrate();
      }

      if (!roleManager.Roles.Any())
      {
        foreach(var role in SD.Roles)
        {
          await roleManager.CreateAsync(new AppRole { Name = role });
        }
      }
      if (!userManager.Users.Any())
      {
        var admin = new AppUser
        {
          Name = "Admin",
          Email = "admin@example.com",
          UserName = "admin",
        };

        await userManager.CreateAsync(admin, "Password123");
        await userManager.AddToRolesAsync(admin, [SD.AdminRole, SD.UserRole, SD.ModeratorRole]);

        var almin = new AppUser
        {
          Name = "Almin",
          Email = "almin@example.com",
          UserName = "almin",
        };

        await userManager.CreateAsync(almin, "Password123");
        await userManager.AddToRoleAsync(almin, SD.UserRole);

        var mirza = new AppUser
        {
          Name = "Mirza",
          Email = "mirza@example.com",
          UserName = "mirza",
        };

        await userManager.CreateAsync(mirza, "Password123");
        await userManager.AddToRoleAsync(mirza, SD.ModeratorRole);
      }
    }
  }
}
