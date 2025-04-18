﻿using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using VideoShare.Core.Entities;
using VideoShare.Core.IRepo;
using VideoShare.DataAccess.Data;
using VideoShare.DataAccess.Repo;
using VideoShare.Services;
using VideoShare.Services.IServices;

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

      builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
      builder.Services.AddScoped<IPhotoService, PhotoService>();
      builder.Services.AddSession();

      return builder;
    }

    public static WebApplicationBuilder AddAuthenticationServices(this WebApplicationBuilder builder)
    {
      builder.Services.AddIdentityCore<AppUser>(options =>
      {
        options.Password.RequiredLength = 6;
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.SignIn.RequireConfirmedEmail = false;
        options.Lockout.AllowedForNewUsers = false;
      }).AddRoles<AppRole>()
      .AddRoleManager<RoleManager<AppRole>>()
      .AddSignInManager<SignInManager<AppUser>>()
      .AddUserManager<UserManager<AppUser>>()
      .AddEntityFrameworkStores<Context>();

      builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
          options.ExpireTimeSpan = TimeSpan.FromHours(24);
          options.LoginPath = "/Account/Login";
          options.AccessDeniedPath = "/Account/AccessDenied";
        });

      return builder;
    }
  }
}
