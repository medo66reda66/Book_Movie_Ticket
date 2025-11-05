using Book_Movie_Ticket.Models;
using ECommerce.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Book_Movie_Ticket.Areas.Identity.controllers
{
    [Area("Identity")]
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ProfileController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return NotFound();

            var userVM = new ApplicationUserVM()
            {
                PhoneNumber = user.PhoneNumber,
                FullName = $"{user.Firestname} {user.Lasttname}",
                Email = user.Email,
                Address = user.Addrress,
            };

            return View(userVM);
        }
        [HttpPost]
        public async Task<IActionResult> Updatepassword(ApplicationUserVM applicationUserVM)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return NotFound();

            if(applicationUserVM.CurrentPassword is null || applicationUserVM.NewPassword is null)
            {
                ModelState.AddModelError(string.Empty, "must have value");
                return RedirectToAction("Index");
            }

            var result = await _userManager.ChangePasswordAsync(user, applicationUserVM.CurrentPassword, applicationUserVM.NewPassword);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "invalid password");
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
