using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarRentalCore2.Data;
using CarRentalCore2.Models;
using CarRentalCore2.Utility;
using CarRentalCore2.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarRentalCore2.Controllers
{
    public class ServicesController : Controller
    {
        public ServicesController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private readonly ApplicationDbContext _dbContext;

        [Authorize]
        public IActionResult Index(int carId)
        {
            var car = _dbContext.Cars.FirstOrDefault(c => c.Id == carId);
            var model = new CarAndServicesViewModel
            {
                CarId = car.Id,
                Make = car.Make,
                Model = car.Model,
                Style = car.Style,
                Vin = car.Vin,
                Year = car.Year,
                UserId = car.UserId,
                ServiceTypes = _dbContext.ServiceTypes.ToList(),
                PastServices = _dbContext.Services.Where(s => s.carId == carId).ToList()
                                                    .OrderByDescending(s => s.DateAdded),
                Service = new Service()
            };

            return View(model);
        }

        //Get Create
        [Authorize(Roles = SD.AdminEndUser)]
        public IActionResult Create(int carId)
        {
            var car = _dbContext.Cars.FirstOrDefault(c => c.Id == carId);
            var model = new CarAndServicesViewModel
            {
                CarId = car.Id,
                Make = car.Make,
                Model = car.Model,
                Style = car.Style,
                Vin = car.Vin,
                Year = car.Year,
                UserId = car.UserId,
                ServiceTypes = _dbContext.ServiceTypes.ToList(),
                PastServices = _dbContext.Services.Where(s => s.carId == carId).ToList()
                                                    .OrderByDescending(s => s.DateAdded)
                                                    .Take(5),
                Service = new Service()
            };

            return View(model);
        }

        //Post Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles =SD.AdminEndUser)]
        public async Task<IActionResult> Create(CarAndServicesViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.Service.carId = model.CarId;
                model.Service.DateAdded = DateTime.Now;
                _dbContext.Add(model.Service);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Create), new { carId = model.CarId });
            }
            else
            {
                var car = _dbContext.Cars.FirstOrDefault(c => c.Id == model.CarId);
                var newModel = new CarAndServicesViewModel
                {
                    CarId = car.Id,
                    Make = car.Make,
                    Model = car.Model,
                    Style = car.Style,
                    Vin = car.Vin,
                    Year = car.Year,
                    UserId = car.UserId,
                    ServiceTypes = _dbContext.ServiceTypes.ToList(),
                    PastServices = _dbContext.Services.Where(s => s.carId == model.CarId).ToList()
                                                 .OrderByDescending(s => s.DateAdded)
                                                 .Take(5)
                };
                return View(newModel);
            }
        }

        //Get Delete
        [Authorize(Roles = SD.AdminEndUser)]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var service = await _dbContext.Services.Include(s => s.Car)
                                                    .Include(s => s.ServiceType)
                                                    .SingleOrDefaultAsync(s => s.Id == id);

            if (service == null)
                return NotFound();

            return View(service);
        }

        //Post Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = SD.AdminEndUser)]
        public async Task<IActionResult> DeleteService(Service model)
        {
            var serviceId = model.Id;
            var carId = model.carId;

            var service = await _dbContext.Services.SingleOrDefaultAsync(s => s.Id == serviceId);

            if (service == null)
                return NotFound();

            _dbContext.Services.Remove(service);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Create), new { carId = carId });
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