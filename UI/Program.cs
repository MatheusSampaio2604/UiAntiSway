using Application.Services;
using Application.Services.Interface;
using Microsoft.Extensions.Configuration;
using Application.Important_Area;
using Infra.RequestApi.Interface;
using Infra.RequestApi;

namespace UI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

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
                app.UseDeveloperExceptionPage();

                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

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