using Book_Movie_Ticket.data;
using Book_Movie_Ticket.FilterMonieVM;
using Book_Movie_Ticket.Models;
using Book_Movie_Ticket.MovieVM;
using Book_Movie_Ticket.Repository;
using Book_Movie_Ticket.Repository.IRepository;
using Book_Movie_Tickets.Models;
using Book_Movie_Tictet.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Book_Movie_Ticket.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ApplicationDBContext _dbContext;
     
      
        public HomeController(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
           
        }
        
        public IActionResult Index(Filtermovie filtermovie)
        {
            var allMovies = _dbContext.Movies.Include(e=>e.Category).Include(e=>e.Cinema).Include(e=>e.ActorsMovies).AsQueryable();
        
           
            if (filtermovie.name is not null)
            {
                allMovies = allMovies.Where(a => a.Name.Contains(filtermovie.name.Trim()));
                ViewBag.name = filtermovie.name;
            }
            if (filtermovie.minprice is not null)
            {
                allMovies = allMovies.Where(p => p.Price >= filtermovie.minprice);
                ViewBag.minprice = filtermovie.minprice;
            }
            if (filtermovie.maxprice is not null)
            {
                allMovies = allMovies.Where(p => p.Price <= filtermovie.maxprice);
                ViewBag.maxprice = filtermovie.maxprice;
            }
            if (filtermovie.categoryid is not null)
            {
                allMovies = allMovies.Where(e => e.CategoryId == filtermovie.categoryid);
                ViewBag.categoryid = filtermovie.categoryid;
            }
            if (filtermovie.cinemaid is not null)
            {
                allMovies = allMovies.Where(e => e.CinemaId == filtermovie.cinemaid);
                ViewBag.cinemaid = filtermovie.cinemaid;
            }
            if (filtermovie.status)
            {
                allMovies = allMovies.Where(s => s.Status == true);
                ViewBag.status = filtermovie.status;
            }
            var category = _dbContext.Categories;
            ViewBag.category = category.AsQueryable();
            var brand = _dbContext.Cinemas;
            ViewBag.cinema = brand.AsQueryable();
            var Actors = _dbContext.ActorsMovies;
            ViewBag.actors = Actors.ToList();

            return View(allMovies);
         
        }
        
        public async Task<IActionResult> Item(int id,CancellationToken cancellationToken)
        {
           var Movie = await _dbContext.Movies.AsNoTracking().Include(e => e.Category).Include(e => e.Cinema).FirstOrDefaultAsync(e=>e.Id == id, cancellationToken);

            if (Movie == null)
                return NotFound();

            var relatedMovie = await _dbContext.Movies.Where(e=>e.Category == Movie.Category && e.Id != Movie.Id).Skip(0).Take(4).ToListAsync();

            return View(new ReletedMovieVM
            {
                Movie= Movie,
                Movies = relatedMovie
            });
        }
       
    }
}
