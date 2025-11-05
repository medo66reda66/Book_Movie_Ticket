using Book_Movie_Ticket.Models;
using Book_Movie_Ticket.MovieVM;
using Book_Movie_Ticket.Repository.IRepository;
using Book_Movie_Tickets.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Book_Movie_Ticket.Areas.Admin.controllers
{
    [Area("Admin")]
    public class PromotionController : Controller
    {
        private readonly MovieIRepository _movieIRepository;
        private readonly IRepository<Promotion> _promotionRepository;
        private readonly IRepository<Movie> _repositorymovie;

        public PromotionController(MovieIRepository movieIRepository, IRepository<Promotion> promotionRepository, IRepository<Movie> repositorymovie)
        {
            _movieIRepository = movieIRepository;
            _promotionRepository = promotionRepository;
            _repositorymovie = repositorymovie;
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
         var promotion =  await _promotionRepository.GetAllAsync(tracked:false,cancellationToken:cancellationToken);
            return View(promotion);
        }
        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
           
            return View(new PromotionVM
            {
                Promotion = new Promotion(),
                Movies = await _movieIRepository.GetAllAsync(cancellationToken: cancellationToken)
            });
        }
        [HttpPost]
        public async Task<IActionResult> Create(Promotion promotion, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.promotion = await _promotionRepository.GetAllAsync(tracked: false, cancellationToken: cancellationToken);
                return View(promotion);
            }
            await _promotionRepository.AddAsync(promotion, cancellationToken);
            await _promotionRepository.commitASync(cancellationToken);
            return RedirectToAction("index","Cart");
        }
    }
}
