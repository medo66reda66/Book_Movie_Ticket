using Book_Movie_Tickets.Models;
using Book_Movie_Tictet.data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Book_Movie_Tictet.Controllers
{
    [Area("Admin")]
    public class CinemaController : Controller
    {
        private ApplicationDBContext _dbContext = new ApplicationDBContext();
        public IActionResult Index()
        {
            var cinema = _dbContext.Cinemas.AsQueryable();
            return View(cinema.AsEnumerable());
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View(new Cinema());
        }
        [HttpPost]
        public IActionResult Create(Cinema cinema ,IFormFile Img)
        {
            if (cinema is not null && Img is not null)
            {
                var filename = Guid.NewGuid().ToString() + Path.GetExtension(Img.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", filename);
                using (var stream = System.IO.File.Create(filePath))
                {
                    Img.CopyTo(stream);
                }
                cinema.Img = filename;

                _dbContext.Cinemas.Add(cinema);
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cinema);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var cinema = _dbContext.Cinemas.FirstOrDefault(e => e.Id == id);
            if (cinema is null)
            {
                return NotFound();
            }
            return View(cinema);
        }
        [HttpPost]
        public IActionResult Edit(Cinema cinema, IFormFile? Img)
        {
            var existingCinema = _dbContext.Cinemas.AsNoTracking().FirstOrDefault(c => c.Id == cinema.Id);
            if (cinema is not null)
            {
                if (Img is not null)
                {
                    var filename = Guid.NewGuid().ToString() + Path.GetExtension(Img.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", filename);
                    using (var stream = System.IO.File.Create(filePath))
                    {
                        Img.CopyTo(stream);
                    }
                    var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", existingCinema.Img);
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                    cinema.Img = filename;
                }
                else
                {
                    cinema.Img = existingCinema.Img;
                }
                    _dbContext.Cinemas.Update(cinema);
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cinema);
        }
        public IActionResult Delete(int id)
        {
            var cinema = _dbContext.Cinemas.FirstOrDefault(e => e.Id == id);
            if (cinema is not null)
            {
                _dbContext.Cinemas.Remove(cinema);
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return NotFound();
        }
    }
}
