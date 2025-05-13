using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserManagementApp.Data;
using Microsoft.AspNetCore.Http;
using UserManagementApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;



namespace UserManagementApp.Controllers {
    public class AccountController : Controller {
        private readonly AppDbContext _dbContext;

        public AccountController(AppDbContext dbContext) {
            _dbContext = dbContext;
        }


        public IActionResult Login() {
            return View();
        }

       [HttpPost]
public async Task<IActionResult> Login(string email, string password) {
    var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email && u.Password == password);

    if (user == null) {
        TempData["Error"] = "Invalid email or password.";
        return RedirectToAction("Login");
    }

    if (user.Status == "blocked") {
        TempData["Error"] = "Your account is blocked.";
        return RedirectToAction("Login");
    }
user.LastLoginTime = DateTime.Now;
    _dbContext.Update(user);
    await _dbContext.SaveChangesAsync();


            HttpContext.Session.SetString("UserEmail", user.Email);
            HttpContext.Session.SetString("UserName", user.Name);
            

    var claims = new List<Claim> {
        new Claim(ClaimTypes.Name, user.Name),
        new Claim(ClaimTypes.Email, user.Email),
        new Claim("UserId", user.UserId.ToString())
    };


    var claimsIdentity = new ClaimsIdentity(claims, "CookieAuth");

   
    await HttpContext.SignInAsync("CookieAuth", new ClaimsPrincipal(claimsIdentity));

    TempData["Message"] = $"Welcome back, {user.Name}!";
    return RedirectToAction("Index", "Admin");
}

    
       
        public IActionResult Register() {
            return View();
        }

     
        [HttpPost]
        public async Task<IActionResult> Register(string name, string email, string password) {
            
            var existingUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (existingUser != null) {
                TempData["Error"] = "The email is already registered.Please use a different email";
                return RedirectToAction("Register");
            }

         
            var user = new User {
                Name = name,
                Email = email,
                Password = password,
                Status = "active", 
                RegistrationTime = DateTime.Now
            };

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            TempData["Message"] = "Registration successful! Please log in.";
            return RedirectToAction("Login");
        }

      
        public IActionResult Logout() {
            HttpContext.Session.Clear(); 
            TempData["Message"] = "You have been logged out successfully.";
            return RedirectToAction("Login");
        }
    }
}
