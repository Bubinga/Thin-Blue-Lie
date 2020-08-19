using System;
using System.Security.Claims;
using System.Threading.Tasks;
using DataAccessLibrary.DataAccess;
using DataAccessLibrary.DataModels;
using DataAccessLibrary.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ThinBlueLie.Pages;


namespace ThinBlueLie.Controllers
{

    public class AccountController : Controller
    {
        private readonly UserContext _usercontext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(UserContext usercontext,                       
                                 UserManager<IdentityUser> userManager,
                                 SignInManager<IdentityUser> signInManager)
        {
            _usercontext = usercontext;            
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public void Log(uint action, string idUser)
        {
            var log = new Log { };
            log.TimeStamp = DateTime.Now; // yyyy-mm-dd hh-mm-ss
            log.Action = action;
            log.IpAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            log.UserId = idUser;
           // log.IdUser = User.FindFirstValue(ClaimTypes.NameIdentifier);
           // log.IdTimelineinfo = 8;
          //  _datacontext.Log.Add(log);
           // _datacontext.SaveChanges();
            //  return Ok(true);
        }


        public async Task<IActionResult> Logout()
        {
            var iduser = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _signInManager.SignOutAsync();
            Log(5, iduser);
            return RedirectToPage("/Index");
        }


        [HttpGet]        
        public IActionResult Login()
        {            
            return View("Pages/Account/Login.cshtml");           
        }

        [HttpPost]
        [Route("Account/Login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var signedUser = await _userManager.FindByEmailAsync(model.Email);
                var result = await _signInManager.PasswordSignInAsync(signedUser, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {      
                    Log((int)LogEnums.ActionEnum.Login, signedUser.Id);
                    return RedirectToPage("/Index");
                }                              
                    ModelState.AddModelError(string.Empty, "Invalid Email or Password");                
            }
            return View("Pages/Account/Login.cshtml", model);
        }


        [HttpGet]
        [Route("Account/Register")]
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
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);                   
                    Log((int)LogEnums.ActionEnum.Register, user.Id);
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
