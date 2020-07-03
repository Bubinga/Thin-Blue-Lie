using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ThinBlueLie.Pages;


namespace ThinBlueLie.Controllers
{
    
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;

        public AccountController(UserManager<IdentityUser> userManager,
                                 SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToPage("/Index");
        }


        [HttpGet]
        public IActionResult Login()
        {            
            return View("Pages/Account/Login.cshtml");           
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {                    
                    return RedirectToPage("/Index");
                }                              
                    ModelState.AddModelError(string.Empty, "Invalid Email or Pass");                
            }
            return View("Pages/Account/Login.cshtml", model);
        }


        [HttpGet]        
        public IActionResult Register()
        {            
            return View("Pages/Account/Register.cshtml");
        }
        [HttpPost]
        [Route("Account/Register")]
        public async Task<IActionResult> Register(RegisterModel model)
        {            
            if (ModelState.IsValid)
            {
                var user = new IdentityUser {UserName = model.Username, Email = model.Email};
                var result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, isPersistent: false); //change isPersistent to true later
                    return RedirectToPage("/Index");
                }
                foreach (var error in result.Errors)
                {
                   ModelState.AddModelError("", error.Description);
                }
            }
            return View("Pages/Account/Register.cshtml", model);
        }
    }
}
