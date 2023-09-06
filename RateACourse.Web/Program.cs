using RateACourse.Core.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using RateACourse.Core.Entities;
using RateACourse.Core.Services;
using RateACourse.Core.Services.Interfaces;

namespace RateAMovie_opl_Afst
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddSingleton<IEmailService, EmailService>();
            builder.Services.AddTransient<IAccountService, AccountService>();
            builder.Services.AddTransient<ICourseService, CourseService>();
            //register HttpContextAccessor
            builder.Services.AddHttpContextAccessor();
            //Add entity framework database
            builder.Services.AddDbContext<ApplicationDbContext>(
                 options => options.UseSqlServer(builder.Configuration.GetConnectionString("CourseRateDb")));
            //add identity
            builder.Services.AddIdentity<ApplicationUser,IdentityRole>(options => 
            {
                //configure options for testing purposes
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.SignIn.RequireConfirmedEmail = true;
            }).AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
            name: "AccountAreaRegister",
            pattern: "Account/Register",
            defaults: new { Area = "Account", Controller = "Account", Action = "Register" });
            app.MapControllerRoute(
            name: "AccountAreaLogin",
            pattern: "Account/Login",
            defaults: new { Area = "Account", Controller = "Account", Action = "Login" });
            app.MapControllerRoute(
                name: "Areas",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}