using Book_Movie_Ticket.Models;
using Book_Movie_Ticket.Repository;
using Book_Movie_Ticket.Repository.IRepository;
using Book_Movie_Tickets.Models;
using Book_Movie_Tictet.Models;

namespace Book_Movie_Ticket
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<IRepository<Category>, Repository<Category>>();
            builder.Services.AddScoped<IRepository<Cinema>, Repository<Cinema>>();
            builder.Services.AddScoped<IRepository<Actors>, Repository<Actors>>();
            builder.Services.AddScoped<IRepository<Movie>, Repository<Movie>>();
            builder.Services.AddScoped<IRepository<ActorsMovie>, Repository<ActorsMovie>>();
            builder.Services.AddScoped<IRepository<MovieSupimg>, Repository<MovieSupimg>>();
            builder.Services.AddScoped<movieSupimgIRepository, movieSupimgRepository>();
            builder.Services.AddScoped<MovieIRepository, MovieReposiory>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{area=Admin}/{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
