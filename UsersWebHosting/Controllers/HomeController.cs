using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Diagnostics;
using System.Security.Claims;
using UsersWebHosting.Data;
using UsersWebHosting.Data.Models;
using UsersWebHosting.Models;

namespace UsersWebHosting.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ApplicationDbContext _dbContext;

        public HomeController(ILogger<HomeController> logger,
            UserManager<User> userManager,
            ApplicationDbContext dbContext,
            SignInManager<User> signInManager)
        {
            _logger = logger;
            _userManager = userManager;
            _dbContext = dbContext;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            var model = new HomePageModel
            {
                IsUserSignedIn = _signInManager.IsSignedIn(User)
            };

            var count = _userManager.Users.Count();

            if (count > 0)
            {
                var isUserAdmin = User.IsInRole("Admin");
                var claim = User.FindFirst(ClaimTypes.NameIdentifier);
                var currentUserId = claim != null ? claim.Value : null;

                model.Users = _userManager.Users.Select(u => new UserItem
                {
                    UserName = u.UserName,
                    UserId = u.Id,
                    ShowAction = isUserAdmin && u.Id != currentUserId
                }).ToList();
            }

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult GetImage(string id)
        {
            var avatarImage = _dbContext.Avatars.FirstOrDefault(a => a.UserId == id);

            if (avatarImage == null)
            {
                return null;
            }

            return File(avatarImage.ImageData, avatarImage.ContentType, avatarImage.FileName);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            if (!string.IsNullOrWhiteSpace(userId))
            {
                var user = await _userManager.FindByIdAsync(userId);
                
                if (user == null)
                {
                    return NotFound();
                }

                var result = await _userManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}