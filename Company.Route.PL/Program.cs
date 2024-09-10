using Company.Route.BLL.Interfaces;
using Company.Route.BLL.Repsitories;
using Company.Route.DAL.Data.Contexts;
using Company.Route.DAL.Models;
using Microsoft.EntityFrameworkCore;

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
            });

            builder.Services.AddScoped<IDepartmentRespstory, DepartmentRepository>();  

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

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
