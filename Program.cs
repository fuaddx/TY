using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Pustok2.Contexts;
using Pustok2.Helpers;
using Pustok2.Models;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();


        builder.Services.AddDbContext<PustokDbContext>(option =>
        {
            option.UseSqlServer(builder.Configuration.GetConnectionString("MSSql"));
            //or option.UseSqlServer(builder.Configuration[GetConnectionString:"MSSql"]);
        }).AddIdentity<AppUser, IdentityRole>(opt =>
        {
            //1
            opt.SignIn.RequireConfirmedEmail = true; 
            //2
            opt.User.RequireUniqueEmail = true;
            //3
            opt.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyz0123456789._";
            opt.Lockout.MaxFailedAccessAttempts = 5;
            opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
            opt.Password.RequireNonAlphanumeric= false;
            opt.Password.RequiredLength = 4;    
        }).AddDefaultTokenProviders().AddEntityFrameworkStores<PustokDbContext>();



        builder.Services.AddSession();
        // ne vaxt Pustok istesem konstruktorda New la ver mene 

        builder.Services.AddScoped<LayoutService>();
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
        }

        app.UseHttpsRedirection();

        app.UseSession();

        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "Areas",
            pattern: "{area:exists}/{controller=Slider}/{action=Index}/{id?}"
        );
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");


        PathConstants.RootPath = builder.Environment.WebRootPath;

        app.Run();
    }
}