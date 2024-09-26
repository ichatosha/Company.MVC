using Company.Route.DAL.Models;
using Company.Route.PL.Helpers;
using Company.Route.PL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;

namespace Company.Route.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;

		public AccountController(
            UserManager<ApplicationUser>    UserManager,
            SignInManager<ApplicationUser>  signInManager
            )
        {
            _userManager = UserManager;
			_signInManager = signInManager;
		}


		#region SignUp
		[HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel model)
        {
            // Mapping => SignUpViewModel To ApplicationUser
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.UserName);
                if (user is null)
                {
					user = await _userManager.FindByEmailAsync(model.Email);
                    if (user is null)
                    {
						user = new ApplicationUser()
                        {
                            UserName = model.UserName,
                            Email = model.Email,
                            FirstName = model.FirstName,
                            LastName = model.LastName,
							IsAgree = model.IsAgree,
                            
                        };
                        // take the model.password to hash 
                        var result = await _userManager.CreateAsync(user, model.Password);
                        if (result.Succeeded)
                        {
                            return RedirectToAction(nameof(SignIn));
                        }
                        foreach(var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                        return View();
                    }
                    ModelState.AddModelError(string.Empty, "Email is already exist");
                    return View(model);
                }
                ModelState.AddModelError(string.Empty, "User Name is already exist");
            }
			return View();
        }

		#endregion


		#region SignIn
		public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async  Task<IActionResult> SignIn(SignInViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
					var user = await _userManager.FindByEmailAsync(model.Email);
					if (user is not null)
					{
						// 1. Check Password 
						var Flag = await _userManager.CheckPasswordAsync(user, model.Password);
						if (Flag is true)
						{
                            // Make Cookie Or Token
                            var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                            if (result.Succeeded)
                            {
							    // Sign In
							    return RedirectToAction("Index", "Home");
                            }
						}
					}
					// model is valid but Email is not Found in database : 
					ModelState.AddModelError(string.Empty, "Invalid LogIn !");
				}
                catch(Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(model);
        }
		#endregion


        #region SignOut
        public new async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("SignIn","Account");
        }

		#endregion


		#region Forget Password
		[HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendResetPasswordUrl(ForgetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userManager.FindByEmailAsync(model.Email);
                if(result is not null)
                {

                    // Create Token
                    var token = await _userManager.GeneratePasswordResetTokenAsync(result);

                    // Create Reset Password URL 
                    var url = Url.Action("ResetPassword", "Account", new { model.Email , token } ,Request.Scheme );
					//https://localhost:44303/Account/ResetPassword?email=hesham@gmail.com&token

					var email = new Email()
                    {
                        To = model.Email,
                        Subject = "Reset Password",
                        Body = url
                    };
                    // Send Email
                    EmailSettings.SendEmail(email);

                    return RedirectToAction(nameof(CheckYourInbox));
                }
                ModelState.AddModelError(string.Empty, "Invalid Operation, Please try again !");
            }
            return View(model);
        }

        public IActionResult CheckYourInbox()
        {
            return View();
        }
		#endregion

		
        #region Reset Password
		[HttpGet]
        public IActionResult ResetPassword(string email , string token)
        {
            TempData["email"] = email;
            TempData["token"] = token;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
				var email = TempData["email"] as string;
				var token = TempData["token"] as string;

				var user = await _userManager.FindByEmailAsync(email);
                if(user is not null)
                {
                   var result = await _userManager.ResetPasswordAsync(user, token, model.Password);

                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(SignIn));
                    }

                }
				
			}
			ModelState.AddModelError(string.Empty, "Invalid Operation, Please try again !");

			return View();
        }
        #endregion


        #region Access Denied 

        public IActionResult AccessDenied()
        {
            return View();
        }

        #endregion


    }
}
