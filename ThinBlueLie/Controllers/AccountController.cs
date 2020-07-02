using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ThinBlueLie.Pages;
using ThinBlueLie.ViewModels;

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
        

        [HttpGet]
        public IActionResult Login()
        {            
            return View("Pages/Account/Login.cshtml");           
        }        


        [HttpGet]
        
        public IActionResult Register()
        {            
            return View("Pages/Account/Register.cshtml");
        }
        [HttpPost]
        [Route("Account/Register")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);
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
            return View(model);
        }
    }
}
