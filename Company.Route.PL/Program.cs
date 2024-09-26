using Company.Route.BLL.Interfaces;
using Company.Route.BLL.Repsitories;
using Company.Route.DAL.Data.Contexts;
using Company.Route.DAL.Models;
using Company.Route.PL.Mapping;
using Company.Route.PL.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Company.Route.PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // allow Depencies Injection for AppDbContext
            //builder.Services.AddSingleton<AppDbContext>();

            // allow DI for DepartmentRepository
            

            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
                //options.UseSqlServer.GetConnectionString("DefaultConnection").logto
            });

            builder.Services.AddScoped<IDepartmentRespstory, DepartmentRepository>();  
            builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();

            // The Diffrence between the three ways is Life Time
            //builder.Services.AddScoped();       // LifeTime Per Request , then Object UnReachable
            //builder.Services.AddTransient();    // LifeTime Per Operations 
            //builder.Services.AddSingleton();    // LifeTime Per Apllication

            builder.Services.AddScoped<IScopedService, ScopedService>();            // LifeTime Per Request    [Best For DbContext Class , Repository Class]
            builder.Services.AddTransient<ITransientService, TransientService>();   // LifeTime Per Operation
            builder.Services.AddSingleton<ISingletonService, SingletonService>();   // LifeTime Per Application

            //enable Auto Mapper :: many overloads
            //builder.Services.AddAutoMapper(typeof(EmployeeProfile));
            //builder.Services.AddAutoMapper(M => M.AddProfile(new EmployeeProfile()));
            builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());            // [Recommended]

            // allow DI for userManager And Sotres: 
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.ConfigureApplicationCookie(config =>
            {
                config.LoginPath = "/Account/SignIn";
                config.AccessDeniedPath = "/Account/AccessDenied";
            });

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
                name: "default",
                pattern: "{controller=Account}/{action=SignIn}/{id?}");

            app.Run();
        }
    }
}
