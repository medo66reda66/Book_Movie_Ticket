using Book_Movie_Ticket.data;
using Book_Movie_Ticket.Repository;
using Book_Movie_Ticket.Repository.IRepository;
using Book_Movie_Ticket.Utilities;
using Book_Movie_Tictet.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Book_Movie_Ticket.Areas.Admin.controllers
{
    [Area("Admin")]
    [Authorize(Roles = $"{SD.SUPER_ADMIN_ROLE},{SD.ADMIN_ROLE},{SD.EMPLOYEE_ROLE}")]
    public class CategoryController : Controller
    {
        //private ApplicationDBContext _context = new ApplicationDBContext();
        private readonly IRepository<Category> _db ;

        public CategoryController(IRepository<Category> db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var catecory = await _db.GetAllAsync(cancellationToken:cancellationToken);
            return View(catecory.AsEnumerable());
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View(new Category());
        }
        [HttpPost]
        public async Task<IActionResult> Create(Category category , CancellationToken cancellationToken)
        {
            try
            {
                if (category is not null)
                {
                    await _db.AddAsync(category, cancellationToken);
                    await _db.commitASync(cancellationToken);
                    //_context.Categories.Add(category);
                    //_context.SaveChanges();
                    return RedirectToAction("Index");
                }
                TempData["sucess-Notification"] = "Product Created Successfully";
            }
            catch (Exception ex)
            {
                TempData["error-Notification"] = "Product Created error";
            }
            return View(category);
        }
        [HttpGet]
        [Authorize(Roles = $"{SD.SUPER_ADMIN_ROLE},{SD.ADMIN_ROLE}")]
        public async Task<IActionResult> Edit(int id , CancellationToken cancellationToken)
        {

            var category = await _db.GetoneAsync(e => e.Id == id, cancellationToken:cancellationToken);
            if (category is null)
            {
                return NotFound();
            }
            return View(category);
        }
        [HttpPost]
        [Authorize(Roles = $"{SD.SUPER_ADMIN_ROLE},{SD.ADMIN_ROLE}")]
        public async Task<IActionResult> Edit(Category category,CancellationToken cancellationToken)
        {
            if (category is not null)
            {
                _db.Updat(category);
                await _db.commitASync(cancellationToken);
            //    _context.Categories.Update(category);
            //    _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }
        [Authorize(Roles = $"{SD.SUPER_ADMIN_ROLE},{SD.ADMIN_ROLE}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var category = await _db.GetoneAsync(e => e.Id == id, cancellationToken: cancellationToken);
            if (category is not null)
            {
                _db.Delete(category);
                 await _db.commitASync(cancellationToken);
                //_context.Categories.Remove(category);
                //_context.SaveChanges();
                return RedirectToAction("Index");
            }
            return NotFound();
        }
    }
}
