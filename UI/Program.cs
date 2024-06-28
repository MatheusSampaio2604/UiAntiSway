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

            //builder.Services.AddAuthorization(opt =>
            //{
            //    opt.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
            //    opt.AddPolicy("UserOnly", policy => policy.RequireRole("User"));
            //});

            //builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //.AddJwtBearer(options =>
            //{
            //    options.Authority = String.Concat(builder.Configuration.GetSection("Mgmt:Route").Value, builder.Configuration.GetSection("Mgmt:Port").Value);
            //    options.Audience = "your-website.com";
            //    options.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateIssuer = true,
            //        ValidateAudience = true,
            //        ValidateLifetime = true,
            //        ValidateIssuerSigningKey = true,
            //        ValidIssuer = String.Concat(builder.Configuration.GetSection("Mgmt:Route").Value, builder.Configuration.GetSection("Mgmt:Port").Value),
            //        ValidAudience = "https://localhost:7026/",
            //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("&secret-key&4002-8922$FromJanusAutomation$"))
            //    };

            //    options.Events = new JwtBearerEvents
            //    {
            //        OnMessageReceived = context =>
            //        {
            //            context.Token = context.Request.Cookies["token"];
            //            return Task.CompletedTask;
            //        }
            //    };
            //});

            //builder.Services.Configure<ApiRouteMgmt>(builder.Configuration.GetSection("Mgmt"));
            //builder.Services.Configure<ApiRoutePlc>(builder.Configuration.GetSection("Plc"));

            builder.Services.AddHttpClient<InterGeneralApi, GeneralApi>();

            builder.Services.AddScoped<InterLoginService, LoginService>();
            builder.Services.AddScoped<InterRegisterService, RegisterService>();
            builder.Services.AddScoped<InterCalibrationService, CalibrationService>();
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

            //app.UseAuthorization();
            //app.UseAuthentication();

            app.MapControllerRoute(
                name: "default",
                //pattern: "{controller=Home}/{action=Index}/{id?}");
                pattern: "{controller=Management}/{action=Monitoring}/{id?}");

            app.UseCors();

            app.Run();
        }
    }
}