using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using SuFood.Models;
using SuFood.ViewModel;
using System.Linq;
using System.Security.Claims;

namespace SuFood.Controllers
{
    public class UserController : Controller
    {
        private readonly SuFoodDBContext _context;
        public UserController(SuFoodDBContext context)
        {
            this._context = context;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return PartialView();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {


            if (Request.Form.ContainsKey("loginButton"))
            {
                var user = _context.Account.FirstOrDefault
                (acc => acc.Account1 == model.Account1 && acc.Password == model.Password);

                if (user == null)  //判斷登入是否成功
                {
                    ViewBag.error = "登入失敗";
                    return PartialView("Login");
                }
                else
                {
                    var claims = new List<Claim>()
                    {
                         new Claim(ClaimTypes.Name, user.Account1),
                         new Claim(ClaimTypes.Role, user.IsSupervisor ? "Employee" : "Customer")
                    };
                    
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    //設定Cookie驗證
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                    //登入
                    await HttpContext.SignInAsync(claimsPrincipal);

                    return RedirectToAction("Index", "Home");// 回到首頁


                }
            }
            else if (Request.Form.ContainsKey("googleButton"))
            {
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Home");

        }

    }
}
