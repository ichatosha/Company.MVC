using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Company.Route.DAL.Models;
using Company.Route.PL.Helpers;
using Company.Route.PL.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Company.Route.BLL.Interfaces;
using Company.Route.BLL.Repsitories;
using Microsoft.AspNetCore.Authorization;
namespace Company.Route.PL.Controllers
{
    [Authorize(Roles = "Admin")]
	public class UserController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;

		// Get , GetAll , Add , delete , update 
		// Index, Details,-   , delete , edit 

		public UserController(UserManager<ApplicationUser> userManager)
        {
			_userManager = userManager;
		}


        #region Get All
        #region Gives Error
        //      public async Task<IActionResult> Index(string WordSearch)
        //{
        //	var users = Enumerable.Empty<UserViewModel>();

        //	if (string.IsNullOrEmpty(WordSearch))
        //	{
        //		users = await _userManager.Users.Select(U => new UserViewModel()
        //		{
        //			Id = U.Id,
        //			FirstName = U.FirstName,
        //			LastName = U.LastName,
        //			Email = U.Email,
        //			Roles = _userManager.GetRolesAsync(U).Result
        //		}).ToListAsync();
        //	}
        //	else
        //	{
        //		users = await _userManager.Users.Where(U => U.Email.ToLower().Contains(WordSearch.ToLower()))
        //			.Select(U => new UserViewModel()
        //			{
        //				Id = U.Id,
        //				FirstName = U.FirstName,
        //				LastName = U.LastName,
        //				Email = U.Email,
        //				Roles = _userManager.GetRolesAsync(U).Result
        //			}).ToListAsync();
        //	}



        //	return View(users);
        //}

        #endregion

        public async Task<IActionResult> Index(string WordSearch)
        {
            var users = Enumerable.Empty<UserViewModel>();

            if (string.IsNullOrEmpty(WordSearch))
            {
                users = await _userManager.Users.Select(U => new UserViewModel()
                {
                    Id = U.Id,
                    FirstName = U.FirstName,
                    LastName = U.LastName,
                    Email = U.Email
                    // Do not get roles here
                }).ToListAsync();
            }
            else
            {
                users = await _userManager.Users
                    .Where(U => U.Email.ToLower().Contains(WordSearch.ToLower()))
                    .Select(U => new UserViewModel()
                    {
                        Id = U.Id,
                        FirstName = U.FirstName,
                        LastName = U.LastName,
                        Email = U.Email
                        // Do not get roles here
                    }).ToListAsync();
            }

            // Fetch roles for each user separately
            foreach (var user in users)
            {
                var applicationUser = await _userManager.FindByIdAsync(user.Id);
                user.Roles = await _userManager.GetRolesAsync(applicationUser);
            }

            return View(users);
        }

        #endregion


        #region Details
        [HttpGet]
        public async Task<IActionResult> Details(string? id, string ViewName = "Details")
        {

            if (id is null) return BadRequest(); //400

			var userFromDb = await _userManager.FindByIdAsync(id);
			if (userFromDb is null)
				return NotFound();  // 404


			var user = new UserViewModel()
			{
				Id = userFromDb.Id,
				FirstName = userFromDb.FirstName,
				LastName = userFromDb.LastName,
				Email = userFromDb.Email,
				Roles = _userManager.GetRolesAsync(userFromDb).Result
			};
            return View(ViewName, user);
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
        public async Task<IActionResult> Update(string? id, UserViewModel model)
        {
            if (id != model.Id) return BadRequest();

			if (ModelState.IsValid)
			{

				var userFromDb = await _userManager.FindByIdAsync(id);

				if (userFromDb is null)
				{
					return NotFound();
				}

				userFromDb.FirstName = model.FirstName;
				userFromDb.LastName = model.LastName;
				userFromDb.Email = model.Email;

				var result =  await _userManager.UpdateAsync(userFromDb);
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
        public async Task<IActionResult> Delete([FromRoute] string? id, UserViewModel model)
        {
            if (id is null) return BadRequest();


            if (ModelState.IsValid)
            {
                var userFromDb = await _userManager.FindByIdAsync(id);

				if (userFromDb is null) 
					return NotFound();

                userFromDb.FirstName = model.FirstName;
                userFromDb.LastName = model.LastName;
                userFromDb.Email = model.Email;

                var result = await _userManager.DeleteAsync(userFromDb);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            // else : stay in model
            return View(model);
        }
        #endregion



    }
}
