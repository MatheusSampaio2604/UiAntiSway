using Application.Services;
using Application.Services.Interface;
using Microsoft.Extensions.Configuration;
using Application.Important_Area;
using Infra.RequestApi.Interface;
using Infra.RequestApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace UI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddAuthorization(opt =>
            {
                opt.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
                opt.AddPolicy("UserOnly", policy => policy.RequireRole("User"));
            });

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = String.Concat(builder.Configuration.GetSection("Mgmt:Route").Value, builder.Configuration.GetSection("Mgmt:Port").Value);
                options.Audience = "your-website.com";
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = String.Concat(builder.Configuration.GetSection("Mgmt:Route").Value, builder.Configuration.GetSection("Mgmt:Port").Value),
                    ValidAudience = "https://localhost:7026/",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("&secret-key&4002-8922$FromJanusAutomation$"))
                };
            
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies["token"];
                        return Task.CompletedTask;
                    }
                };
            });

            //builder.Services.Configure<ApiRouteMgmt>(builder.Configuration.GetSection("Mgmt"));
            //builder.Services.Configure<ApiRoutePlc>(builder.Configuration.GetSection("Plc"));

  

            builder.Services.AddHttpClient<InterGeneralApi, GeneralApi>();

            builder.Services.AddScoped<InterLoginService, LoginService>();
            builder.Services.AddScoped<InterRegisterService, RegisterService>();
            builder.Services.AddScoped<InterCalibrationService, CalibrationService>();
            builder.Services.AddScoped<InterConfigurationService, ConfigurationService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            else
            {
                app.UseHsts();
            }
                app.UseExceptionHandler("/Home/Error");

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}