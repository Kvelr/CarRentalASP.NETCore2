using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarRentalCore2.Data;
using CarRentalCore2.Models;
using CarRentalCore2.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarRentalCore2.Controllers
{
    [Authorize(Roles = SD.AdminEndUser)]
    public class UsersController : Controller
    {
        public UsersController(ApplicationDbContext dbConext)
        {
            _dbContext = dbConext;
        }

        private readonly ApplicationDbContext _dbContext;


        //Get: Users
        public IActionResult Index(string option = null, string search = null)
        {
            var users = _dbContext.Users.ToList();

            if (!string.IsNullOrWhiteSpace(search))
            {
                switch (option)
                {
                    case SD.email:
                        Func<ApplicationUser, bool> emailPredicate = u => u.Email.ToLower().Contains(search.ToLower());
                        users = FilterList(users, emailPredicate);
                        break;

                    case SD.name:
                        Func<ApplicationUser, bool> namePredicate = u => u.FirstName.ToLower().Contains(search.ToLower()) ||
                                                                     u.LastName.ToLower().Contains(search.ToLower());
                        users = FilterList(users, namePredicate);
                        break;

                    case SD.phone:
                        Func<ApplicationUser, bool> phonePredicate = u => u.PhoneNumber.ToLower().Contains(search.ToLower());
                        users = FilterList(users, phonePredicate);
                        break;
                }
            }

            return View(users);
        }

        //Get: Users/Details
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return NotFound();

            ApplicationUser userToFind = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (userToFind == null)
                return NotFound();

            return View(userToFind);
        }

        //Get: Users/Edit
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return NotFound();

            ApplicationUser userToFind = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (userToFind == null)
                return NotFound();

            return View(userToFind);
        }

        //post: User/Edit/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ApplicationUser user)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", user);
            }
            else
            {
                var userToEdit = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == user.Id);

                if (userToEdit == null)
                    return NotFound();

                userToEdit.FirstName = user.FirstName;
                userToEdit.LastName = user.LastName;
                userToEdit.Email = user.Email;
                userToEdit.PhoneNumber = user.PhoneNumber;
                userToEdit.Address = user.Address;
                userToEdit.City = user.City;
                userToEdit.PostalCode = user.PostalCode;

                _dbContext.Update(userToEdit);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
        }

        //Get: Users/Delete
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return NotFound();

            ApplicationUser userToFind = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (userToFind == null)
                return NotFound();

            return View(userToFind);
        }

        //Post: Users/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfrim(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return NotFound();

            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
                return NotFound();

            var cars = _dbContext.Cars.Where(c => c.UserId == user.Id).ToList();

            foreach (var car in cars)
            {
                var services = _dbContext.Services.Where(s => s.carId == car.Id).ToList();

                _dbContext.Services.RemoveRange(services);
            }

            _dbContext.Cars.RemoveRange(cars);
            _dbContext.Users.Remove(user);

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private List<T> FilterList<T>(IEnumerable<T> users, Func<T, bool> predicate)
        {
            return users.Where(predicate).ToList();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dbContext.Dispose();
            }
        }
    }
}