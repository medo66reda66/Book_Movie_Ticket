using Book_Movie_Ticket.data;
using Book_Movie_Ticket.Repository;
using Book_Movie_Ticket.Repository.IRepository;
using Book_Movie_Ticket.Utilities;
using Book_Movie_Tickets.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Threading.Tasks;

namespace Book_Movie_Tictet.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = $"{SD.SUPER_ADMIN_ROLE},{SD.ADMIN_ROLE},{SD.EMPLOYEE_ROLE}")]
    public class CinemaController : Controller
    {
        //private ApplicationDBContext _dbContext = new ApplicationDBContext();
        private readonly IRepository<Cinema> _db ;

        public CinemaController(IRepository<Cinema> db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var cinema = await _db.GetAllAsync(cancellationToken:cancellationToken);
            return View(cinema.AsEnumerable());
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View(new Cinema());
        }
        [HttpPost]
        public async Task<IActionResult> Create(Cinema cinema ,IFormFile Img , CancellationToken cancellationToken)
        {
            try
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

                    await _db.AddAsync(cinema, cancellationToken);
                    await _db.commitASync(cancellationToken);

                    //_dbContext.Cinemas.Add(cinema);
                    //_dbContext.SaveChanges();
                    return RedirectToAction("Index");
                    
                }
                TempData["sucess-Notification"] = "Product Created Successfully";
            }
            catch (Exception ex) 
            {
                TempData["error-Notification"] = "Product Created error";
            }
            return View(cinema);
        }
        [HttpGet]
        [Authorize(Roles = $"{SD.SUPER_ADMIN_ROLE},{SD.ADMIN_ROLE}")]
        public async Task<IActionResult> Edit(int id , CancellationToken cancellationToken )
        {
            var cinema = await _db.GetoneAsync(e => e.Id == id,tracked:false, cancellationToken:cancellationToken);
            if (cinema is null)
            {
                return NotFound();
            }
            return View(cinema);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Cinema cinema, IFormFile? Img , CancellationToken cancellationToken)
        {
            var existingCinema = await _db.GetoneAsync(c => c.Id == cinema.Id,tracked:false,cancellationToken:cancellationToken);
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

                _db.Updat(cinema);
                await _db.commitASync(cancellationToken);
                //    _dbContext.Cinemas.Update(cinema);
                //_dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cinema);
        }
        [Authorize(Roles = $"{SD.SUPER_ADMIN_ROLE},{SD.ADMIN_ROLE}")]
        public async Task<IActionResult> Delete(int id,CancellationToken cancellationToken)
        {
            var cinema = await _db.GetoneAsync(e => e.Id == id,cancellationToken:cancellationToken);
            if (cinema is not null)
            {
                _db.Delete(cinema);
                 await _db.commitASync(cancellationToken);
                //_dbContext.Cinemas.Remove(cinema);
                //_dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return NotFound();
        }
    }
}
