using Book_Movie_Tickets.Models;
using Book_Movie_Tictet.data;
using Book_Movie_Tictet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Book_Movie_Tictet.Controllers
{
    [Area("Admin")]
    public class ActorsController : Controller
    {

        private ApplicationDBContext _context = new ApplicationDBContext();
        public IActionResult Index()
        {
            var actors = _context.Actors.AsQueryable();
            return View(actors.AsEnumerable());
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View(new Actors());
        }
        [HttpPost]
        public IActionResult Create(Actors actor,IFormFile img)
        {
            if (actor is not null && img is not null)
            {
                var filename = Guid.NewGuid().ToString() + Path.GetExtension(img.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Actorimg", filename);
                using (var stream = System.IO.File.Create(filePath))
                {
                    img.CopyTo(stream);
                }
                actor.Img = filename;
            

                _context.Actors.Add(actor);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(actor);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var actor = _context.Actors.FirstOrDefault(e => e.Id == id);
            if (actor is null)
            {
                return NotFound();
            }
            return View(actor);
        }
        [HttpPost]
        public IActionResult Edit(Actors actor, IFormFile? img)
        {
            var existingActor = _context.Actors.AsNoTracking().FirstOrDefault(c => c.Id == actor.Id);
            if (actor is not null)
            {
                if (img is not null)
                {
                    var filename = Guid.NewGuid().ToString() + Path.GetExtension(img.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Actorimg", filename);
                    using (var stream = System.IO.File.Create(filePath))
                    {
                        img.CopyTo(stream);
                    }
                    var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Actorimg", existingActor.Img);
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                    actor.Img = filename;
                }
                else
                {
                    actor.Img = existingActor.Img;
                }
                _context.Actors.Update(actor);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(actor);
        }
        public IActionResult Delete(int id)
        {
            var actor = _context.Actors.FirstOrDefault(e => e.Id == id);
            if (actor is not null)
            {
                _context.Actors.Remove(actor);
                _context.SaveChanges();

        
                return RedirectToAction("Index");
            }
            return NotFound();
        }
    }
}
