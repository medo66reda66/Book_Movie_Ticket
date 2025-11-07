using Book_Movie_Ticket.Models;
using Book_Movie_Ticket.Repository.IRepository;
using Book_Movie_Tickets.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using System.Threading;
using System.Threading.Tasks;

namespace Book_Movie_Ticket.Areas.Customer.controllers
{
    [Area("Customer")]
    public class CartController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRepository<Cart> _cartRepository;
        private readonly IRepository<Promotion> _promotionRepository;
        private readonly IRepository<Movie> _movieRepository;

        public CartController(UserManager<ApplicationUser> userManager, IRepository<Cart> cartRepository, IRepository<Promotion> promotionRepository, IRepository<Movie> movieRepository)
        {
            _userManager = userManager;
            _cartRepository = cartRepository;
            _promotionRepository = promotionRepository;
            _movieRepository = movieRepository;
        }



        public async Task<IActionResult> Index(string code,CancellationToken cancellationToken)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user is null)
                return NotFound();

            var cart = await _cartRepository.GetAllAsync(e => e.ApplicationUserId == user.Id, includ: [e=>e.Movie,e=>e.Movie.Category,e => e.ApplicationUser]);

            var promotion = await _promotionRepository.GetoneAsync(e => e.Code == code && e.Isvalid);
            if (promotion is not null)
            {
                var cartitem = cart.FirstOrDefault(e => e.Movieid == promotion.Movieid);
                if (cartitem is not null)
                {
                    var discountAmount = cartitem.Price * (promotion.Discount / 100);
                    cartitem.Price -= discountAmount;
                }
               await _cartRepository.commitASync(cancellationToken);
            }

            return View(cart.AsEnumerable());
        }
        [HttpPost]
        public async Task<IActionResult> AddToCart(int count, int Movieid, CancellationToken cancellationToken)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) 
                return NotFound();

            var Movieindb = await _cartRepository.GetoneAsync(e=>e.Movieid == Movieid && e.ApplicationUserId == user.Id, cancellationToken:cancellationToken);

            if(Movieindb is not null)
            {
                Movieindb.count += count;
                return RedirectToAction("Index","Home");
            }
            await _cartRepository.AddAsync(new Cart
            {
                Movieid = Movieid,
                count = count,
                ApplicationUserId = user.Id,
                Price =(decimal)(await _movieRepository.GetoneAsync(e=>e.Id == Movieid, cancellationToken:cancellationToken)).Price
            },cancellationToken);

           await _cartRepository.commitASync(cancellationToken);

            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> Increment(int Movieid,CancellationToken cancellationToken)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var movie = await _cartRepository.GetoneAsync(e=>e.Movieid == Movieid && e.ApplicationUserId == user.Id);
            if (movie is  null) 
                return NotFound();

            movie.count += 1;
            await _cartRepository.commitASync(cancellationToken);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Decrement(int Movieid, CancellationToken cancellationToken)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var movie = await _cartRepository.GetoneAsync(e=>e.Movieid == Movieid && e.ApplicationUserId == user.Id);
            if (movie is  null) 
                return NotFound();
            if(movie.count < 1)
            {
                _cartRepository.Delete(movie);
            }

            movie.count -= 1;
            await _cartRepository.commitASync(cancellationToken);
            return RedirectToAction("Index");

        }
        public async Task<IActionResult> Delete(int movieid,CancellationToken cancellationToken)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var movie = await _cartRepository.GetoneAsync(e=>e.Movieid == movieid && e.ApplicationUserId == user.Id);
            if (movie is  null) 
                return NotFound();

            _cartRepository.Delete(movie);
            await _cartRepository.commitASync(cancellationToken);
            return RedirectToAction("Index");

        }

        public async Task<IActionResult> Pay()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user is null)
                return NotFound();

            var cart = await _cartRepository.GetAllAsync(e => e.ApplicationUserId == user.Id, includ: [e => e.Movie]);

            if (cart is null) return NotFound();

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = $"{Request.Scheme}://{Request.Host}/customer/checkout/success",
                CancelUrl = $"{Request.Scheme}://{Request.Host}/customer/checkout/cancel",
            };

            foreach (var item in cart)
            {
                options.LineItems.Add(new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "egp",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Movie.Name,
                            Description = item.Movie.Description,
                        },
                        UnitAmount = (long)item.Price * 100,
                    },
                    Quantity = item.count,
                });
            }

            var service = new SessionService();
            var session = service.Create(options);
            return Redirect(session.Url);
        }


    }
}
