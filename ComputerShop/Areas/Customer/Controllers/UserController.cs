using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ComputerShop.Data;
using ComputerShop.Models;

namespace ComputerShop.Areas.Customer.Controllers
{

    [Area("Customer")]
    [Authorize]
    public class UserController : Controller
    {
        private ApplicationDbContext _db;

        UserManager<IdentityUser> _userManager;

        public UserController(UserManager<IdentityUser> userManager, ApplicationDbContext db)
        {
            _db = db;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View(_db.ApplicationUsers.ToList());
        }


        //CREATE NEW USER
        [AllowAnonymous]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Create(ApplicationUser user)
        {
            if(ModelState.IsValid)
            {
                var result = await _userManager.CreateAsync(user, user.PasswordHash);

                if(result.Succeeded)
                {
                    var isSaveRole = await _userManager.AddToRoleAsync(user, "User");

                    TempData["save"] = "User created successfully";

                    return RedirectToAction(nameof(Index), "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View();
        }
        //CREATE NEW USER


        //EDIT USER INFO
        public async Task<IActionResult> Edit(string id)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(c => c.Id == id);
            if(user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ApplicationUser user)
        {
            var userInfo = _db.ApplicationUsers.FirstOrDefault(c => c.Id == user.Id);

            if(userInfo == null)
            {
                return NotFound();
            }

            userInfo.FirstName = user.FirstName;
            userInfo.LastName = user.LastName;

            var result = await _userManager.UpdateAsync(userInfo);

            if(result.Succeeded)
            {
                TempData["save"] = "User updated successfully";

                return RedirectToAction(nameof(Index));
            }

            return View();
        }
        //EDIT USER INFO

        //USER DETAILS
        public async Task<IActionResult> Details(string id)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(c => c.Id == id);

            if(user == null)
            {
                return NotFound();
            }

            return View(user);
        }
        //USER DETAILS

        //DEACTIVATE USER TEMPORARILY
        public async Task<IActionResult> Deactivate(string id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var user = _db.ApplicationUsers.FirstOrDefault(c => c.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Deactivate(ApplicationUser user)
        { 
            var userInfo = _db.ApplicationUsers.FirstOrDefault(c => c.Id == user.Id);

            if (userInfo == null)
            {
                return NotFound();
            }

            userInfo.LockoutEnd = DateTime.Now.AddYears(100);

            int rowAffected = _db.SaveChanges();

            if(rowAffected > 0)
            {
                TempData["save"] = "User has been deactivated successfully";

                return RedirectToAction(nameof(Index));
            }

            return View(user);
        }
        //DEACTIVATE USER TEMPORARILY

        //ACTIVATE DEACTIVATED USER
        public async Task<IActionResult> Activate(string id)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(c => c.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Activate(ApplicationUser user)
        {
            var userInfo = _db.ApplicationUsers.FirstOrDefault(c => c.Id == user.Id);

            if (userInfo == null)
            {
                return NotFound();
            }

            userInfo.LockoutEnd = null;

            int rowAffected = _db.SaveChanges();

            if (rowAffected > 0)
            {
                TempData["save"] = "User Activated Successfully";

                return RedirectToAction(nameof(Index));
            }

            return View(user);
        }
        //ACTIVATE DEACTIVATED USER

        //PERMANENTLY DELETE USER
        public async Task<IActionResult> Delete(string id)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(c => c.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePermanent(ApplicationUser user)
        {
            var userInfo = _db.ApplicationUsers.FirstOrDefault(c => c.Id == user.Id);
            if (userInfo == null)
            {
                return NotFound();
            }

            _db.ApplicationUsers.Remove(userInfo);
            int rowAffected = _db.SaveChanges();
            if (rowAffected > 0)
            {
                TempData["save"] = "User Delete Successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }
        //PERMANENTLY DELETE USER
    }
}

