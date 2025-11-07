using Book_Movie_Ticket.data;
using Book_Movie_Ticket.Models;
using Book_Movie_Ticket.Repository;
using Book_Movie_Ticket.Repository.IRepository;
using Book_Movie_Ticket.Utilities;
using Book_Movie_Tickets.Models;
using Book_Movie_Tictet.Models;
using ECommerce.Utitlies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Stripe;

namespace Book_Movie_Ticket
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<ApplicationDBContext>(option =>
            {
                //option.UseSqlServer(builder.Configuration.GetSection("ConnectionStrings")["default"]);
                //option.UseSqlServer(builder.Configuration["ConnectionStrings : default"]);
                option.UseSqlServer(builder.Configuration.GetConnectionString("default"));


            });

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(Option =>
            {
                Option.Password.RequiredLength = 6;
                Option.Password.RequireLowercase = false;
                Option.Password.RequireUppercase = false;
                Option.Password.RequireNonAlphanumeric = false;
                Option.User.RequireUniqueEmail = true;
                Option.SignIn.RequireConfirmedEmail = true;

            })
            .AddEntityFrameworkStores<ApplicationDBContext>()
            .AddDefaultTokenProviders();

            builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));
            StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];

            builder.Services.AddTransient<IEmailSender, Emailsender>();

            builder.Services.AddScoped<IRepository<Category>, Repository<Category>>();
            builder.Services.AddScoped<IRepository<Cinema>, Repository<Cinema>>();
            builder.Services.AddScoped<IRepository<Actors>, Repository<Actors>>();
            builder.Services.AddScoped<IRepository<Movie>, Repository<Movie>>();
            builder.Services.AddScoped<IRepository<Promotion>, Repository<Promotion>>();
            builder.Services.AddScoped<IRepository<ActorsMovie>, Repository<ActorsMovie>>();
            builder.Services.AddScoped<IRepository<ApplicationuserOtp>, Repository<ApplicationuserOtp>>();
            builder.Services.AddScoped<IRepository<MovieSupimg>, Repository<MovieSupimg>>();
            builder.Services.AddScoped<IRepository<Cart>, Repository<Cart>>();
            builder.Services.AddScoped<movieSupimgIRepository, movieSupimgRepository>();
            builder.Services.AddScoped<MovieIRepository, MovieReposiory>();
            builder.Services.AddScoped<IBBInitializer, DBInitializer>();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Identity/Account/Login"; // Default login path
                options.AccessDeniedPath = "/Identity/Account/AccessDenied"; // Default access denied path
            });



            var app = builder.Build();
            var scope = app.Services.CreateScope();
            var service = scope.ServiceProvider.GetService<IBBInitializer>();
            service!.DBInitializ();

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
                pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
