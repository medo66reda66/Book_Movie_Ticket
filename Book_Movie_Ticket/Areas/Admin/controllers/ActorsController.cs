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
    public class ActorsController : Controller
    {

        //private ApplicationDBContext _context = new ApplicationDBContext();
        private readonly IRepository<Actors> _db ;

        public ActorsController(IRepository<Actors> db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var actors = await _db.GetAllAsync(cancellationToken:cancellationToken);
            return View(actors.AsEnumerable());
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View(new Actors());
        }
        [HttpPost]
        public async Task<IActionResult> Create(Actors actor,IFormFile img,CancellationToken cancellationToken)
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
            
                await _db.AddAsync(actor,cancellationToken);
                await _db.commitASync(cancellationToken);
                //_context.Actors.Add(actor);
                //_context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(actor);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id,CancellationToken cancellationToken)
        {
            var actor = await _db.GetoneAsync(e => e.Id == id,cancellationToken:cancellationToken);
            if (actor is null)
            {
                return NotFound();
            }
            return View(actor);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Actors actor, IFormFile? img,CancellationToken cancellationToken)
        {
            var existingActor = await _db.GetoneAsync(c => c.Id == actor.Id,tracked:false  ,cancellationToken:cancellationToken);
            try
            {
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
                    _db.Updat(actor);
                    await _db.commitASync(cancellationToken);

                    //_context.Actors.Update(actor);
                    //_context.SaveChanges();
                    
                    TempData["sucess-Notification"] = "Product Created Successfully";
                    return RedirectToAction("Index");
                }
            } catch (Exception ex)
            {
                TempData["error-Notification"] = "Product Created error";
            }
            return View(actor);
        }
        public async Task<IActionResult> Delete(int id,CancellationToken cancellationToken)
        {
            var actor = await _db.GetoneAsync(e => e.Id == id,cancellationToken:cancellationToken);
            if (actor is not null)
            {
                _db.Delete(actor);
                await _db.commitASync(cancellationToken);
                //_context.Actors.Remove(actor);
                //_context.SaveChanges();

        
                return RedirectToAction("Index");
            }
            return NotFound();
        }
    }
}
