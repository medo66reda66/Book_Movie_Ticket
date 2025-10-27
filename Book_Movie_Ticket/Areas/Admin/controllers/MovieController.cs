using Book_Movie_Ticket.Models;
using Book_Movie_Ticket.MovieVM;
using Book_Movie_Ticket.Repository;
using Book_Movie_Ticket.Repository.IRepository;
using Book_Movie_Tickets.Models;
using Book_Movie_Tictet.data;
using Book_Movie_Tictet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Book_Movie_Tictet.Controllers
{
    [Area("Admin")]
    public class MovieController : Controller
    {
        //private ApplicationDBContext _context = new ApplicationDBContext();
        private readonly IRepository<Cinema> _cinemaRepository;
        private readonly IRepository<Category> _CategoryRepository;
        private readonly IRepository<Actors> _ActorsRepository;
        private readonly IRepository<Movie> _MovieRepository;
        private readonly IRepository<MovieSupimg> _MovieSupimgRepository;
        private readonly IRepository<Movie> _movieRepository;
        private readonly IRepository<ActorsMovie> _ActorsMovieRepository;
        private readonly MovieIRepository _movieIRepository;
        private readonly movieSupimgIRepository _movieSupimgIRepository;

        public MovieController(IRepository<Cinema> cinemaRepository, IRepository<Category> categoryRepository, IRepository<Actors> actorsRepository,
            IRepository<MovieSupimg> movieSupimgRepository, IRepository<Movie> movieRepository, IRepository<ActorsMovie> actorsMovieRepository,
            MovieIRepository movieIRepository, movieSupimgIRepository movieSupimgIRepository)
        {
            _cinemaRepository = cinemaRepository;
            _CategoryRepository = categoryRepository;
            _ActorsRepository = actorsRepository;
            _MovieSupimgRepository = movieSupimgRepository;
            _movieRepository = movieRepository;
            _ActorsMovieRepository = actorsMovieRepository;
            _movieIRepository = movieIRepository;
            _movieSupimgIRepository = movieSupimgIRepository;
        }

        public async Task<IActionResult> Index( CancellationToken cancellationToken)
        {
            var allMovies = await _movieIRepository.GetAllAsync(includ:[e => e.Actors,e => e.Supimg,c => c.Cinema,e=>e.Category,a=>a.ActorsMovies],cancellationToken:cancellationToken);
            var actormovie = await _ActorsMovieRepository.GetAllAsync(includ: [p => p.Actor],cancellationToken: cancellationToken);

            //var allMovies = _context.Movies.Include(e => e.Category)
            //    .Include(e => e.Actors).Include(e => e.Supimg)
            //    .Include(c => c.Cinema).Include(a => a.ActorsMovies)
            //    .ThenInclude(e=>e.Actor).AsQueryable();

            var categories = await _CategoryRepository.GetAllAsync(cancellationToken:cancellationToken);
            var cinemas = await _cinemaRepository.GetAllAsync(cancellationToken: cancellationToken);
            var actors = await _ActorsRepository.GetAllAsync(cancellationToken: cancellationToken);
            var supimg = await _MovieSupimgRepository.GetAllAsync(cancellationToken: cancellationToken);
            return View(new MovieVM
            {
                Movies = allMovies.AsEnumerable(),
                Movie = new Movie(),
                Categories = categories.AsEnumerable(),
                Cinemas = cinemas.AsEnumerable(),
                Actors = actors.AsEnumerable(),
                MovieSupimg = supimg.AsEnumerable(),
                MovieSupimgs = new MovieSupimg(),
                ActorsMovieS = new List<ActorsMovie>(),
            });
        }
        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken cancellationToken )
        {
            //var allMovies = _context.Movies.Include(e => e.Category).Include(c => c.Cinema).AsQueryable();
            var allMovies = await _movieIRepository.GetAllAsync(includ:[e => e.Category, e => e.Cinema],cancellationToken:cancellationToken);
            var categories = await _CategoryRepository.GetAllAsync(cancellationToken:cancellationToken);
            var cinemas = await _cinemaRepository.GetAllAsync(cancellationToken: cancellationToken);
            var actors = await _ActorsRepository.GetAllAsync(cancellationToken: cancellationToken);
            var supimg = await _MovieSupimgRepository.GetAllAsync(cancellationToken: cancellationToken);
            var Actormovie = await _ActorsMovieRepository.GetAllAsync(includ: [e => e.Actor],cancellationToken:cancellationToken);
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
        public async Task<IActionResult> Create(Movie movie, IFormFile MainImg, List<IFormFile>? SupImg,List<int> Actors,CancellationToken cancellationToken)
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
                var movieid = await _movieIRepository.AddAsync(movie,cancellationToken:cancellationToken);
                  await _movieIRepository.commitASync(cancellationToken);

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
                        await _MovieSupimgRepository.AddAsync(new MovieSupimg
                        {
                            MovieId = movieid.Id,
                            SupImg = filename,
                        },cancellationToken);
                        
                        await _MovieSupimgRepository.commitASync(cancellationToken:cancellationToken);
                    }
                }
                foreach (var actor in Actors)
                { 
                   await _ActorsMovieRepository.AddAsync(new ActorsMovie
                    {
                        ActorId = actor,
                        MovieId = movieid.Id,
                    },cancellationToken);
                await _ActorsMovieRepository.commitASync(cancellationToken: cancellationToken);
                  }
                
                return RedirectToAction("Index");
            }
            return View(movie);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id ,CancellationToken cancellationToken)
        {
            //var movie = _context.Movies.Include(E=>E.ActorsMovies).FirstOrDefault(m => m.Id == id);
            var movie = await _movieIRepository.GetoneAsync(m => m.Id == id,includ: [E => E.ActorsMovies ], cancellationToken: cancellationToken);
            if (movie is null)
            {
                return NotFound();
            }
            var categories = await _CategoryRepository.GetAllAsync(cancellationToken:cancellationToken);
            var cinemas = await _cinemaRepository.GetAllAsync(cancellationToken: cancellationToken);
            var actors = await _ActorsRepository.GetAllAsync(cancellationToken: cancellationToken);
            var supimg = await _MovieSupimgRepository.GetAllAsync(e => e.MovieId == movie.Id ,cancellationToken:cancellationToken);
            //var actorsMovie = _context.ActorsMovies.Where(e => e.MovieId == movie.Id).Include(e => e.Actor).AsQueryable();
            var actorsMovie = await _ActorsMovieRepository.GetoneAsync(e => e.MovieId == movie.Id, includ: [e => e.Actor], cancellationToken);
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
        public async Task<IActionResult> Edit(Movie movie, IFormFile MainImg, List<IFormFile>? SupImg,CancellationToken cancellationToken)
        {
            //var existingMovie = _context.Movies.AsNoTracking().FirstOrDefault(m => m.Id == movie.Id);
            var existingMovie = await _movieIRepository.GetoneAsync(m => m.Id == movie.Id, tracked: false, cancellationToken: cancellationToken);
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
                _movieIRepository.Updat(movie);
                await _movieIRepository.commitASync(cancellationToken);
                //_context.Movies.Update(movie);
                //_context.SaveChanges();
                if (SupImg is not null && SupImg.Count > 0)
                {

                    //var oldSupImgs = _context.MovieSupimgs.Where(ms => ms.MovieId == movie.Id).ToList();
                    var oldSupImgs = await _movieSupimgIRepository.GetAllAsync(ms => ms.MovieId == movie.Id , cancellationToken:cancellationToken) ;
                    
                    foreach (var old in oldSupImgs)
                    {
                        var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Moviesupimg", old.SupImg);
                        if (System.IO.File.Exists(oldPath))
                        {
                            System.IO.File.Delete(oldPath);
                        }
                    }
                    _movieSupimgIRepository.RemoveRange(oldSupImgs);
                     await _movieSupimgIRepository.commitASync(cancellationToken );

                    //_context.MovieSupimgs.RemoveRange(oldSupImgs);
                    //_context.SaveChanges();
                     
                    foreach (var supImg in SupImg)
                    {
                        var filename = Guid.NewGuid().ToString() + Path.GetExtension(supImg.FileName);
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Moviesupimg", filename);

                        using (var stream = System.IO.File.Create(path))
                        {
                            supImg.CopyTo(stream);
                        }

                        await _MovieSupimgRepository.AddAsync(  new MovieSupimg
                        {
                            MovieId = movie.Id,
                            SupImg = filename
                        },cancellationToken);
                        
                    }
                    await _MovieSupimgRepository.commitASync(cancellationToken);
                }
              
                return RedirectToAction("Index");
            }
            return View(movie);
        }
        public async Task<IActionResult> Delete(int id ,CancellationToken cancellationToken)
        {
            var movie = await _movieIRepository.GetoneAsync(m => m.Id == id, includ:[m=>m.ActorsMovies] , cancellationToken:cancellationToken);
            var moviesupimg = await _movieSupimgIRepository.GetAllAsync(m => m.MovieId == id, cancellationToken: cancellationToken);

            if (movie is null && moviesupimg is null )
            {
                return NotFound();
            }
            _movieSupimgIRepository.RemoveRange(moviesupimg);
            _movieIRepository.Delete(movie);
            await _movieIRepository.commitASync(cancellationToken:cancellationToken);
            //_context.Movies.Remove(movie);
            //_context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
