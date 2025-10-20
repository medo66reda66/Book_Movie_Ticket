using Book_Movie_Ticket.Models;
using Book_Movie_Ticket.MovieVM;
using Book_Movie_Tickets.Models;
using Book_Movie_Tictet.data;
using Book_Movie_Tictet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Book_Movie_Tictet.Controllers
{
    [Area("Admin")]
    public class MovieController : Controller
    {
        private ApplicationDBContext _context = new ApplicationDBContext();
        public IActionResult Index()
        {
            var allMovies = _context.Movies.Include(e => e.Category)
                .Include(e => e.Actors).Include(e => e.Supimg)
                .Include(c => c.Cinema).Include(a => a.ActorsMovies)
                .ThenInclude(e=>e.Actor).AsQueryable();

            var categories = _context.Categories.AsQueryable();
            var cinemas = _context.Cinemas.AsQueryable();
            var actors = _context.Actors.AsQueryable();
            var supimg = _context.MovieSupimgs.AsQueryable();
            return View(new MovieVM
            {
                Movies = allMovies.AsEnumerable(),
                Movie = new Movie(),
                Categories = categories.AsEnumerable(),
                Cinemas = cinemas.AsEnumerable(),
                Actors = actors.AsEnumerable(),
                MovieSupimg = supimg.AsEnumerable(),
                MovieSupimgs = new MovieSupimg(),
                ActorsMovieS = new List<ActorsMovie>()
            });
        }
        [HttpGet]
        public IActionResult Create()
        {
            var allMovies = _context.Movies.Include(e => e.Category).Include(c => c.Cinema).AsQueryable();
            var categories = _context.Categories.AsQueryable();
            var cinemas = _context.Cinemas.AsQueryable();
            var actors = _context.Actors.AsQueryable();
            var supimg = _context.MovieSupimgs.AsQueryable();
            var Actormovie = _context.ActorsMovies.Include(e => e.Actor).AsQueryable();
            return View(new MovieVM
            {
                Movies = allMovies.AsQueryable(),
                Movie = new Movie(),
                Categories = categories.AsEnumerable(),
                Cinemas = cinemas.AsEnumerable(),
                Actors = actors.AsEnumerable(),
                MovieSupimg = supimg.AsEnumerable(),
                MovieSupimgs = new MovieSupimg(),
                ActorsMovieS  = Actormovie.AsEnumerable()
            });
        }
        [HttpPost]
        public IActionResult Create(Movie movie, IFormFile MainImg, List<IFormFile>? SupImg,List<int> Actors)
        {
            if (movie is not null)
            {
                if (MainImg is not null && MainImg.Length > 0)
                {
                    var filename = Guid.NewGuid().ToString() + Path.GetExtension(MainImg.FileName);
                    var pathname = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Movieimg", filename);
                    using (var stream = System.IO.File.Create(pathname))
                    {
                        MainImg.CopyTo(stream);
                    }
                    movie.MainImg = filename;
                }
                var movieid = _context.Movies.Add(movie);
                _context.SaveChanges();

                if (SupImg is not null && SupImg.Count > 0)
                {
                    foreach (var supImg in SupImg)
                    {
                        var filename = Guid.NewGuid().ToString() + Path.GetExtension(supImg.FileName);
                        var pathname = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Moviesupimg", filename);
                        using (var stream = System.IO.File.Create(pathname))
                        {
                            supImg.CopyTo(stream);
                        }
                        _context.MovieSupimgs.Add(new MovieSupimg
                        {
                            MovieId = movieid.Entity.Id,
                            SupImg = filename,
                        });
                        _context.SaveChanges();
                    }
                }
                foreach (var actor in Actors)
                { 
                    _context.ActorsMovies.Add(new ActorsMovie
                    {
                        ActorId = actor,
                        MovieId = movieid.Entity.Id,
                    });
                _context.SaveChanges();
                  }
                
                return RedirectToAction("Index");
            }
            return View(movie);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var movie = _context.Movies.Include(E=>E.ActorsMovies).FirstOrDefault(m => m.Id == id);
            if (movie is null)
            {
                return NotFound();
            }
            var categories = _context.Categories.AsQueryable();
            var cinemas = _context.Cinemas.AsQueryable();
            var actors = _context.Actors.AsQueryable();
            var supimg = _context.MovieSupimgs.Where(e=>e.MovieId == movie.Id).AsQueryable();
            var actorsMovie = _context.ActorsMovies.Where(e => e.MovieId == movie.Id).Include(e => e.Actor).AsQueryable();
            return View(new MovieVM
            {
                Movie = movie,
                Categories = categories.AsEnumerable(),
                Cinemas = cinemas.AsEnumerable(),
                Actors = actors.AsEnumerable(),
                MovieSupimg = supimg.AsEnumerable(),
                MovieSupimgs = new MovieSupimg(),
                ActorsMovieS = new List<ActorsMovie>()
            });
        }
        [HttpPost]
        public IActionResult Edit(Movie movie, IFormFile MainImg, List<IFormFile>? SupImg)
        {
            var existingMovie = _context.Movies.AsNoTracking().FirstOrDefault(m => m.Id == movie.Id);
            if (movie is not null)
            {
                if (MainImg is not null && MainImg.Length > 0)
                {
                    var filename = Guid.NewGuid().ToString() + Path.GetExtension(MainImg.FileName);
                    var pathname = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Movieimg", filename);
                    using (var stream = System.IO.File.Create(pathname))
                    {
                        MainImg.CopyTo(stream);
                    }
                    var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Movieimg", existingMovie.MainImg);
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                    movie.MainImg = filename;
                }else
                {
                       movie.MainImg = existingMovie.MainImg;
                }
                _context.Movies.Update(movie);
                _context.SaveChanges();
                if (SupImg is not null && SupImg.Count > 0)
                {
                    
                    var oldSupImgs = _context.MovieSupimgs.Where(ms => ms.MovieId == movie.Id).ToList();

                    foreach (var old in oldSupImgs)
                    {
                        var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Moviesupimg", old.SupImg);
                        if (System.IO.File.Exists(oldPath))
                        {
                            System.IO.File.Delete(oldPath);
                        }
                    }
                    _context.MovieSupimgs.RemoveRange(oldSupImgs);
                    _context.SaveChanges();
                     
                    foreach (var supImg in SupImg)
                    {
                        var filename = Guid.NewGuid().ToString() + Path.GetExtension(supImg.FileName);
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Moviesupimg", filename);

                        using (var stream = System.IO.File.Create(path))
                        {
                            supImg.CopyTo(stream);
                        }

                        _context.MovieSupimgs.Add(  new MovieSupimg
                        {
                            MovieId = movie.Id,
                            SupImg = filename
                        });
                        
                    }
                    _context.SaveChanges();
                }
              
                return RedirectToAction("Index");
            }
            return View(movie);
        }
        public IActionResult Delete(int id)
        {
            var movie = _context.Movies.FirstOrDefault(m => m.Id == id);
            var movieSupimgs = _context.MovieSupimgs.Where(ms => ms.MovieId == id).ToList();
            var actorsMovies = _context.ActorsMovies.Where(am => am.MovieId == id).ToList();
            if (movie is null && movieSupimgs is  null && actorsMovies is  null)
            {
                return NotFound();
            }
            _context.MovieSupimgs.RemoveRange(movieSupimgs);
            _context.ActorsMovies.RemoveRange(actorsMovies);
            _context.Movies.Remove(movie);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
