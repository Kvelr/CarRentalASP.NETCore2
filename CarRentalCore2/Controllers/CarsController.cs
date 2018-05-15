using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CarRentalCore2.Data;
using CarRentalCore2.Models;
using CarRentalCore2.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarRentalCore2.Controllers
{
    [Authorize]
    public class CarsController : Controller
    {

        public CarsController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private readonly ApplicationDbContext _dbContext;

        //get index
        public async Task<IActionResult> Index(string userID = null)
        {
            if (string.IsNullOrWhiteSpace(userID))
            {
                //only if guest
                userID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            }

            var model = new CarAndCustomerViewModel()
            {
                Cars = _dbContext.Cars.Where(c => c.UserId == userID).ToList(),
                User = _dbContext.Users.FirstOrDefault(u => u.Id == userID)
            };

            return View(model);
        }

        //Create Get
        public async Task<IActionResult> Create(string userId)
        {
            Car carToAdd = new Car()
            {
                Year = DateTime.Now.Year,
                UserId = userId
            };

            return View(carToAdd);
        }

        //Creat Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Car carToSave)
        {
            if (carToSave == null)
                return NotFound();

            if (ModelState.IsValid)
            {
                _dbContext.Add(carToSave);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { userId = carToSave.UserId });
            }
            return View(carToSave);
        }

        //Details Get
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || id == 0)
                return NotFound();

            var selectedCar = await _dbContext.Cars.Include(u => u.ApplicationUser)
                                    .FirstOrDefaultAsync(c => c.Id == id);

            if (selectedCar == null)
                return NotFound();

            return View(selectedCar);
        }

        //Details Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id == 0)
                return NotFound();

            var selectedCar = await _dbContext.Cars.Include(u => u.ApplicationUser)
                                    .FirstOrDefaultAsync(c => c.Id == id);

            if (selectedCar == null)
                return NotFound();

            return View(selectedCar);
        }

        //Edit Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Car carToEdit)
        {
            if (carToEdit == null || id != carToEdit.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                _dbContext.Update(carToEdit);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { userId = carToEdit.UserId });
            }
            return View(carToEdit);
        }

        //Details Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id == 0)
                return NotFound();

            var selectedCar = await _dbContext.Cars.Include(u => u.ApplicationUser)
                                    .FirstOrDefaultAsync(c => c.Id == id);

            if (selectedCar == null)
                return NotFound();

            return View(selectedCar);
        }

        //Delete Post
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCar(int id)
        {
            var car = await _dbContext.Cars.SingleOrDefaultAsync(c => c.Id == id);

            if (car == null)
                return NotFound();

            _dbContext.Cars.Remove(car);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { userId = car.UserId });
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