using Company.Route.DAL.Models;
using Company.Route.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Company.Route.PL.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {


        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        // Get ,  GetAll , Add ,   delete , update 
        // Index, Details, Create, delete,  update 

        public RoleController(RoleManager<IdentityRole> roleManager , UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        


        #region Get All


        public async Task<IActionResult> Index(string WordSearch)
        {
            var roles = Enumerable.Empty<RoleViewModel>();

            if (string.IsNullOrEmpty(WordSearch))
            {
                roles = await _roleManager.Roles.Select(R => new RoleViewModel()
                {
                    Id = R.Id,
                    RoleName = R.Name

                }).ToListAsync();
            }
            else
            {
                roles = await _roleManager.Roles.Where(R => R.Name.ToLower()
                                                .Contains(WordSearch.ToLower()))
                                                    .Select(R => new RoleViewModel()
                                                    {
                                                        Id = R.Id,
                                                        RoleName = R.Name
                                                    }).ToListAsync();
            }
            return View(roles);
        }

        #endregion


        #region Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(RoleViewModel model)
        {

            if (ModelState.IsValid)
            {
                // mapping from viewModel to IdentityRole
                // because CreateAsync take Parametre kind of IdentityRole
                var role = new IdentityRole()
                {
                    Name = model.RoleName
                };
                var result = await _roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View();
        } 
        #endregion


        #region Details
        [HttpGet]
        public async Task<IActionResult> Details(string? id, string ViewName = "Details")
        {

            if (id is null) return BadRequest(); //400

            var roleFromDb = await _roleManager.FindByIdAsync(id);
            if (roleFromDb is null)
                return NotFound();  // 404


            var role = new RoleViewModel()
            {
                Id = roleFromDb.Id,
                RoleName = roleFromDb.Name
            };
            return View(ViewName, role);
        }
        #endregion


        #region Update
        [HttpGet]
        public async Task<IActionResult> Update(string? id)
        {
            return await Details(id, "Update");
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Update(string? id, RoleViewModel model)
        {
            if (id != model.Id) return BadRequest();

            if (ModelState.IsValid)
            {

                var roleFromDb = await _roleManager.FindByIdAsync(id);

                if (roleFromDb is null)
                {
                    return NotFound();
                }

                roleFromDb.Name = model.RoleName;
                

                var result = await _roleManager.UpdateAsync(roleFromDb);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);

        }

        #endregion


        #region Delete

        [HttpGet]
        public async Task<IActionResult> Delete(string? id)
        {

            return await Details(id, "Delete");

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] string? id, RoleViewModel model)
        {
            if (id is null) return BadRequest();


            if (ModelState.IsValid)
            {
                var roleFromDb = await _roleManager.FindByIdAsync(id);

                if (roleFromDb is null)
                    return NotFound();

                roleFromDb.Id = model.Id;
                roleFromDb.Name = model.RoleName;

                var result = await _roleManager.DeleteAsync(roleFromDb);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            // else : stay in model
            return View(model);
        }
        #endregion


        #region Add Or Remove User
        [HttpGet]
        public async Task<IActionResult> AddOrRemoveUser(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role is null) return NotFound();

            ViewData["RoleId"] = roleId;
            var usersInRole = new List<UsersInRoleViewModel>();


            var users = await _userManager.Users.ToListAsync();

            foreach (var user in users)
            {
                var userInRole = new UsersInRoleViewModel()
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };

                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userInRole.IsSelected = true;
                }
                else
                {
                    userInRole.IsSelected = false;
                }
                usersInRole.Add(userInRole);
            }
            return View(usersInRole);
        }


        [HttpPost]
        public async Task<IActionResult> AddOrRemoveUser(string roleId , List<UsersInRoleViewModel> users)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role is null) return NotFound();

           if(ModelState.IsValid)
           {
                foreach (var user in users)
                {
                    var appUser = await _userManager.FindByIdAsync(user.UserId);
                    if (appUser is not null)
                    {
                        if (user.IsSelected && ! await _userManager.IsInRoleAsync(appUser , role.Name))
                        {
                            await _userManager.AddToRoleAsync(appUser, role.Name);
                        }
                        else if(!user.IsSelected && await _userManager.IsInRoleAsync(appUser , role.Name))
                        {
                            await _userManager.RemoveFromRoleAsync(appUser, role.Name);
                        }
                    }
                }
                // update here take a parameter of Id 
                return RedirectToAction(nameof(Update) , new {id = roleId});
           }
            return View(users);
        }
        #endregion
    }
}
