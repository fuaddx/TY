using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pustok2.Models;
using Pustok2.ViewModel.AuthVM;

namespace Pustok2.Controllers
{
    public class AuthController1 : Controller
    {
        SignInManager<AppUser> _signInManager { get; }
        UserManager<AppUser>_userManager { get; }
        public IActionResult Register()
        {
            return View();
        }
         
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM vm)
        {
            if(!ModelState.IsValid)
            {
                return View(vm);
            }

            var result = await  _userManager.CreateAsync(new AppUser
            {
                Fullname = vm.Fullname,
                Email = vm.Email,
                UserName = vm.Username,
            },vm.Password);

            if(!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(vm);
            }
            return View();
        }
    }
}
