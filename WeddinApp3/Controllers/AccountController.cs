using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeddinApp3.Models;
using WeddinApp3.Models.ViewModels;
using WeddinApp3.Utility;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace WeddinApp3.Controllers
{
    public class AccountController : Controller
    {

        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailSender _emailSender;
        private static int new_count = 3;

        public AccountController(
            ApplicationDbContext db,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager, 
            SignInManager<ApplicationUser> signInManager, 
            IEmailSender emailSender)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }

        public IActionResult Login()
        {
            return View();
        }

        //Login post method called in Login View 
        //Returns model with user details 
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Login(LoginViewModel model)
        {
            {
                if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if(result.Succeeded)
                {
                        //var user = await _userManager.FindByNameAsync(model.Email);
                        //HttpContext.Session.SetString("userID", user.Id);
                        new_count = 3; 
                    return RedirectToAction("Index", "Appointment");
                }

                    if (new_count == 0)
                    {
                        return RedirectToAction(nameof(ForgotPassword));
                    }

                    ModelState.AddModelError("", "Invalid login attempt, " + (new_count) + " attempt" + (new_count > 1 ? "s" : "") + " left.");
                    new_count--;
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Register()
        {
            return View();
        }


        //Register Post method to create a new user 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {

            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    Name = model.Name
                };  

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, model.RoleName);
                    if (!User.IsInRole(Helper.Admin))
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                    }
                    return RedirectToAction("Index", "Appointment");
                }

                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

            }
            return View(model);
        }

        
        [HttpPost]
        //LogOff Post method and redirection to Login page 
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }



        //Get method to return Forgot Password View
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }


        //Post method to generate token for password reset and email link to user 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel forgotPasswordModel)
        {
            if (!ModelState.IsValid)
                return View(forgotPasswordModel);

            var user = await _userManager.FindByEmailAsync(forgotPasswordModel.Email);
            if (user == null)
                return RedirectToAction(nameof(ForgotPasswordConfirmation));

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callback = Url.Action(nameof(ResetPassword), "Account", new { token, email = user.Email }, Request.Scheme);

            await _emailSender.SendEmailAsync(user.Email, "Reset password token", $"Please follow this link, {callback}");

            return RedirectToAction(nameof(ForgotPasswordConfirmation));
        }

        //Get method called in the Forgot Password method 
        [HttpGet]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }


        //method to allow user to reset password, if it is not successfull error message will be presented 
        //if successful reset confirmation displayed in view 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel resetPasswordModel)
        {
            if (!ModelState.IsValid)
                return View(resetPasswordModel);

            var user = await _userManager.FindByEmailAsync(resetPasswordModel.Email);
            if (user == null)
                RedirectToAction(nameof(ResetPasswordConfirmation));

            var resetPassResult = await _userManager.ResetPasswordAsync(user, resetPasswordModel.Token, resetPasswordModel.Password);
            if (!resetPassResult.Succeeded)
            {
                foreach (var error in resetPassResult.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }

                return View();
            }

            return RedirectToAction(nameof(ResetPasswordConfirmation));
        }




        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
            var model = new ResetPasswordModel { Token = token, Email = email };
            return View(model);
        }

        [HttpGet]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }
    }
}
