using Book_Movie_Ticket.Models;
using Book_Movie_Ticket.MovieVM;
using Book_Movie_Ticket.Repository.IRepository;
using Book_Movie_Ticket.Utilities;
using ECommerce.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace Book_Movie_Ticket.Areas.Identity.controllers
{
    [Area("Identity")]

    public class AccountController : Controller
    {
        private readonly Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly IRepository<ApplicationuserOtp> _ApplicationuserOtpRepository;

        public AccountController(Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IEmailSender emailSender, IRepository<ApplicationuserOtp> applicationuserOtpRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _ApplicationuserOtpRepository = applicationuserOtpRepository;
        }


        public async Task<IActionResult> Logout()
        {
           await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid)
                return View(registerVM);

            var user = new ApplicationUser
            {
                Firestname = registerVM.Firstname,
                Lasttname = registerVM.Lastname,
                Email = registerVM.Email,
                PhoneNumber = registerVM.Phone,
                UserName = registerVM.Username
            };

            var result = await _userManager.CreateAsync(user, registerVM.Password);

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var link = Url.Action(nameof(ConfirmEmail), "Account", new { area = "Identity", token, userid = user.Id }, Request.Scheme);

            await _emailSender.SendEmailAsync(registerVM.Email, "Book ticket Movie..Confirm Your Email!",
                $"<h1>Confirm Your Email By Clicking <a href='{link}'>Here</a></h1>");

            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, item.Code);
                }
                return View(registerVM);
            }
            await _userManager.AddToRoleAsync(user,SD.CUSTOMER_ROLE);
            return RedirectToAction(nameof(Login));

        }
        [HttpGet]
        public IActionResult ResendEmailConfirm()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResendEmailConfirm(ResendEmailConfirmVM resendEmailConfirmVM)
        {
            if (!ModelState.IsValid)
                return View(resendEmailConfirmVM);

            var user = await _userManager.FindByEmailAsync(resendEmailConfirmVM.UsernameORemail) ?? await _userManager.FindByNameAsync(resendEmailConfirmVM.UsernameORemail);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "invalid user");
                return View(resendEmailConfirmVM);
            }
            if (user.EmailConfirmed)
            {
                ModelState.AddModelError(string.Empty, "Already Confirmed!!");
                return View(resendEmailConfirmVM);
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var link = Url.Action(nameof(ConfirmEmail), "Account", new { area = "Identity", token, userid = user.Id }, Request.Scheme);

            await _emailSender.SendEmailAsync(user.Email!, "Book ticket Movie..Resend Your Email!",
                $"<h1>Confirm Your Email By Clicking <a href='{link}'>Here</a></h1>");

            return RedirectToAction(nameof(Login));
        }
        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordVm forgetPasswordVm, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return View(forgetPasswordVm);

            var user = await _userManager.FindByNameAsync(forgetPasswordVm.UserNameOREmail) ?? await _userManager.FindByEmailAsync(forgetPasswordVm.UserNameOREmail);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "invalid user");
                return RedirectToAction(nameof(ForgetPassword));
            }

            var otp = new Random().Next(10000, 99999).ToString();

            await _ApplicationuserOtpRepository.AddAsync(new()
            {
                id = Guid.NewGuid().ToString(),
                Applicationuserid = user.Id,
                CreateAt = DateTime.UtcNow,
                Isvalid = true,
                OTP = otp,
                Validto = DateTime.UtcNow.AddDays(1)

            }, cancellationToken);

            await _ApplicationuserOtpRepository.commitASync(cancellationToken);

            await _emailSender.SendEmailAsync(user.Email!, "Book ticket Movie..Resend Your password!",
               $"<h1>Confirm Your Email By Otp : {otp} </h1>");

            return RedirectToAction("ValidateOtp", new { useid = user.Id });
        }
        [HttpGet]
        public async Task<IActionResult> ValidateOtp(string useid)
        {
            return View(new ValidateVM
            {
                ApplicationUserId = useid
            });
        }
        [HttpPost]
        public async Task<IActionResult> ValidateOtp(ValidateVM validateVM)
        {
            var result = await _ApplicationuserOtpRepository.GetoneAsync(e => e.Applicationuserid == validateVM.ApplicationUserId && e.OTP == validateVM.OTP && e.Isvalid);

            if (result is null)
            {
                ModelState.AddModelError(string.Empty, "ivalid OTP");
                return RedirectToAction(nameof(ValidateOtp), new { useid = validateVM.ApplicationUserId });
            }

            return RedirectToAction("NewPassword", new { usrid = result.Applicationuserid });
        }
        [HttpGet]
        public IActionResult NewPassword(string usrid)
        {
            return View(new NewPasswordVM
            {
                ApplicationUserId= usrid
            });
        }
        [HttpPost]
        public async Task<IActionResult> NewPassword(NewPasswordVM newPasswordVM)
        {
            var user = await _userManager.FindByIdAsync(newPasswordVM.ApplicationUserId);
            if (user == null)
            { 
                ModelState.AddModelError(string.Empty, "invalid user");
                return RedirectToAction(nameof(NewPassword));
             }

            var token =await _userManager.GeneratePasswordResetTokenAsync(user);

            var result = await _userManager.ResetPasswordAsync(user, token,newPasswordVM.Password);

            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, item.Code);
                }
                return View(newPasswordVM);
            }
            return RedirectToAction(nameof(Login));
        }



        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userid , string token)
        {
            var user = await _userManager.FindByIdAsync(userid);
            if (user is null)
                TempData["error-notification"] = "Invalid User Cred.";

            var result = await _userManager.ConfirmEmailAsync(user!, token);

            if (!result.Succeeded)
                TempData["error-notification"] = "Invalid OR Expired Token";
            else
                TempData["success-notification"] = "Confirm Email Successfully";

            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(loginVm loginVm)
        {
            if (!ModelState.IsValid)
                return View(loginVm);
            var user = await _userManager.FindByEmailAsync(loginVm.UsernameORemail) ?? await _userManager.FindByNameAsync(loginVm.UsernameORemail);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "invalid username|Email or password");
                return View(loginVm);
            }

           var result = await _signInManager.PasswordSignInAsync(user,loginVm.PaSsword,loginVm.Rememberme,lockoutOnFailure:true);
            if (!result.Succeeded)
            {
                if (result.IsLockedOut)
                    ModelState.AddModelError(string.Empty, "to many attemps try Again after 5 min");
                else if(!user.EmailConfirmed)
                    ModelState.AddModelError(string.Empty, "please confirm Email");
                else
                    ModelState.AddModelError(string.Empty, "invalid username|Email or password");
               
                return View(loginVm);
            }


            return RedirectToAction("index", "Home", new { area = "Customer" });
        }



    }
}
