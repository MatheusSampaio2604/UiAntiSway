using Application.Services;
using Application.Services.Interface;
using Infra.RequestApi;
using Infra.RequestApi.Interface;

namespace UI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.WebHost.UseUrls("http://localhost:5200");

            builder.Services.AddHttpClient<InterGeneralApi, GeneralApi>();

            builder.Services.AddScoped<InterLoginService, LoginService>();
            builder.Services.AddScoped<InterCameraService, CameraService>();
            builder.Services.AddScoped<InterConfigurationService, ConfigurationService>();

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHsts();
            app.UseExceptionHandler("/Home/Error");

            //app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.MapControllerRoute(
                name: "default",
                //pattern: "{controller=Home}/{action=Index}/{id?}");
                pattern: "{controller=Management}/{action=Monitoring}/{id?}");

            app.UseCors();

            app.Run();
        }
    }
}