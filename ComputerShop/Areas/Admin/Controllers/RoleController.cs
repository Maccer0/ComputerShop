using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ComputerShop.Areas.Admin.Models;
using ComputerShop.Data;


namespace ComputerShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class RoleController : Controller
    {
        RoleManager<IdentityRole> _roleManager;
        UserManager<IdentityUser> _userManager;
        ApplicationDbContext _db;

        public RoleController(RoleManager<IdentityRole> roleManager,
            ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            _roleManager = roleManager;
            _db = db;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var roles = _roleManager.Roles.ToList();
            ViewBag.Roles = roles;

            return View();
        }

        //CREATE ROLE
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string name)
        {
            IdentityRole role = new IdentityRole();
            role.Name = name;

            var isTaken = await _roleManager.RoleExistsAsync(name);

            if(!isTaken)
            {
                var result = await _roleManager.CreateAsync(role);

                if(result.Succeeded)
                {
                    TempData["save"] = "Role saved successfully";

                    return RedirectToAction(nameof(Index));
                }
            }

            ViewBag.mgs = "This role already exists";
            ViewBag.name = name;

            return View();
        }
        //CREATE ROLE

        //EDIT ROLE
        public async Task<IActionResult> Edit(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                return NotFound();
            }

            ViewBag.id = role.Id;
            ViewBag.name = role.Name;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, string name)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            role.Name = name;

            var isTaken = await _roleManager.RoleExistsAsync(name);

            if(!isTaken)
            {
                var result = await _roleManager.UpdateAsync(role);

                if(result.Succeeded)
                {
                    TempData["save"] = "Role updated successfully";

                    return RedirectToAction(nameof(Index));
                }
            }

            ViewBag.mgs = "This role already exists";
            ViewBag.name = name;
            return View();
        }
        //EDIT ROLE

        //DELETE ROLE
        public async Task<IActionResult> Delete(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if(role == null)
            {
                return NotFound();
            }

            ViewBag.id = role.Id;
            ViewBag.name = role.Name;

            return View();
        }

        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                return NotFound();
            }

            var result = _roleManager.DeleteAsync(role);

            if(result.IsCompletedSuccessfully)
            {
                TempData["save"] = "Role deleted successfully";

                return RedirectToAction(nameof(Index));
            }

            ViewBag.id = role.Id;
            ViewBag.name = role.Name;

            return View();
        }
        //DELETE ROLE

        //ASSIGN ROLE TO USER
        public async Task<IActionResult> Assign()
        {
            ViewData["UserId"] = new SelectList(_db.ApplicationUsers.ToList(), "Id", "UserName");
            ViewData["RoleId"] = new SelectList(_roleManager.Roles.ToList(), "Name", "Name");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Assign(RoleUserViewModel roleUser)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(c => c.Id == roleUser.UserId);

            var isAssigned = await _userManager.IsInRoleAsync(user, roleUser.RoleId);

            var oldRoles = await _userManager.GetRolesAsync(user);

            if(isAssigned)
            {
                ViewBag.mgs = "This role is already assigned to this user";
                return View();
            }

            foreach (var oldrole in oldRoles)
            {
                await _userManager.RemoveFromRoleAsync(user, oldrole);
            }

            var role = await _userManager.AddToRoleAsync(user, roleUser.RoleId);

            if(role.Succeeded)
            {
                TempData["save"] = "Role assigned successfully";

                return RedirectToAction(nameof(AssignUserRole));
            }

            return View();
        }

        public IActionResult AssignUserRole()
        {
            var result = from userRole in _db.UserRoles
                         join role in _db.Roles on userRole.RoleId equals role.Id
                         join appUser in _db.ApplicationUsers on userRole.UserId equals appUser.Id
                         select new UserRoleMapping()
                         {
                             UserId = userRole.UserId,
                             RoleId = userRole.RoleId,
                             UserName = appUser.UserName,
                             RoleName = role.Name
                         };

            ViewBag.UserRoles = result;

            return View();
        }
    }
}

