using Book_Movie_Ticket.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Book_Movie_Ticket.MovieVM;
using Book_Movie_Tickets.Models;
using Book_Movie_Tictet.Models;
using ECommerce.ViewModels;


namespace Book_Movie_Ticket.data
{
    public class ApplicationDBContext : IdentityDbContext<ApplicationUser>
    {
          public ApplicationDBContext(DbContextOptions<ApplicationDBContext> dbContext) : base(dbContext)
          {

          }
    
            public DbSet<Movie> Movies { get; set; }
            public DbSet<Cinema> Cinemas { get; set; }
            public DbSet<Category> Categories { get; set; }
            public DbSet<Actors> Actors { get; set; }
            public DbSet<MovieSupimg> MovieSupimgs { get; set; }
            public DbSet<ActorsMovie> ActorsMovies { get; set; }
            public DbSet<ApplicationuserOtp> applicationuserOtps { get; set; }
            public DbSet<Promotion> Promotions { get; set; }
            public DbSet<Cart> Carts { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    base.OnConfiguring(optionsBuilder);
        //    optionsBuilder.UseSqlServer("Data Source=.;Initial catalog = Cinema;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");
        //}
        override protected void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Movie>()
             .HasKey(m => m.Id);
            modelBuilder.Entity<Cinema>()
                .HasKey(c => c.Id);
            modelBuilder.Entity<Category>()
                .HasKey(ca => ca.Id);
            modelBuilder.Entity<Actors>()
                .HasKey(a => a.Id);
            modelBuilder.Entity<MovieSupimg>() .
                HasKey(ms => ms.Id);
            modelBuilder.Entity<ActorsMovie>()
                .HasKey(am => new { am.ActorId, am.MovieId });

        }
        //public DbSet<Book_Movie_Ticket.MovieVM.ValidateVM> ValidateVM { get; set; } = default!;
        //public DbSet<ECommerce.ViewModels.NewPasswordVM> NewPasswordVM { get; set; } = default!;
        //public DbSet<Book_Movie_Ticket.MovieVM.RegisterVM> RegisterVM { get; set; } = default!;
        //public DbSet<Book_Movie_Ticket.MovieVM.loginVm> loginVm { get; set; } = default!;
        //public DbSet<Book_Movie_Ticket.MovieVM.ResendEmailConfirmVM> ResendEmailConfirmVM { get; set; } = default!;
        //public DbSet<Book_Movie_Ticket.MovieVM.ForgetPasswordVm> ForgetPasswordVm { get; set; } = default!;
    }
}
