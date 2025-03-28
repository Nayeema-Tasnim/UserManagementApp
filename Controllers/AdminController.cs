using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization; 
using UserManagementApp.Data;

namespace UserManagementApp.Controllers {
    [Authorize]     public class AdminController : Controller {
        private readonly AppDbContext _dbContext;

        public AdminController(AppDbContext dbContext) {
            _dbContext = dbContext;
        }

        // Fetch users for the management table
        [HttpGet]
        public async Task<IActionResult> Index() {
            var users = await _dbContext.Users.OrderBy(u => u.LastLoginTime).ToListAsync();
            return View(users);
        }

        // Block selected users
        [HttpPost]
        public async Task<IActionResult> BlockUsers(int[] userIds) {
            var users = await _dbContext.Users.Where(u => userIds.Contains(u.UserId)).ToListAsync();
            foreach (var user in users) {
                user.Status = "blocked";
            }
            await _dbContext.SaveChangesAsync();
            TempData["Message"] = "Selected users have been blocked.";
            return RedirectToAction("Index");
        }

        // Unblock selected users
        [HttpPost]
        public async Task<IActionResult> UnblockUsers(int[] userIds) {
            var users = await _dbContext.Users.Where(u => userIds.Contains(u.UserId)).ToListAsync();
            foreach (var user in users) {
                user.Status = "active";
            }
            await _dbContext.SaveChangesAsync();
            TempData["Message"] = "Selected users have been unblocked.";
            return RedirectToAction("Index");
        }

        // Delete selected users
        [HttpPost]
        public async Task<IActionResult> DeleteUsers(int[] userIds) {
            var users = await _dbContext.Users.Where(u => userIds.Contains(u.UserId)).ToListAsync();
            _dbContext.Users.RemoveRange(users);
            await _dbContext.SaveChangesAsync();
            TempData["Message"] = "Selected users have been deleted.";
            return RedirectToAction("Index");
        }
    }
}
