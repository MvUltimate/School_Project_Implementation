using DAL;
using Microsoft.EntityFrameworkCore;
using MVC_SchoolProject.Services;
using WebApi_SchoolProject.Services;
using AuthService = MVC_SchoolProject.Services.AuthService;

namespace MVC_SchoolProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddControllersWithViews();
            builder.Services.AddHttpContextAccessor();
            // Register services

            builder.Services.AddHttpClient<AuthService, AuthService>();
            // Add scoped services for AuthService and StudentService
            // Scoped lifetime means services are created once per request within the scope

            builder.Services.AddScoped<MVC_SchoolProject.Services.AuthService>();
            builder.Services.AddScoped<MVC_SchoolProject.Services.StudentService>();
            builder.Services.AddScoped<MVC_SchoolProject.Services.AdminService>();

            // Add session support
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });


            var app = builder.Build();
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();
            //app.MapRazorPages();
            // Set the default route to /login/login
            app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Login}/{action=Login}/{id?}");

            app.Run();
        }
    }
}
