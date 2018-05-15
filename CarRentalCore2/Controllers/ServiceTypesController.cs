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
    public class ServiceTypesController : Controller
    {
        #region C'tor

        public ServiceTypesController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #endregion

        #region Props

        private readonly ApplicationDbContext _dbContext;

        #endregion

        #region Action Method's

        //Get: ServiceTypes
        public IActionResult Index()
        {
            return View(_dbContext.ServiceTypes.ToList());
        }

        //Get: ServiceTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        //Post: ServiceTypes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ServiceType serviceType)
        {
            if (ModelState.IsValid)
            {
                _dbContext.ServiceTypes.Add(serviceType);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(serviceType);
        }

        //Get: ServiceTypes/Details/1
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var serviceType = await _dbContext.ServiceTypes.FirstOrDefaultAsync(s => s.ID == id);

            if (serviceType == null)
                return NotFound();

            return View(serviceType);
        }

        //Get: ServiceTypes/Edit/1
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var serviceType = await _dbContext.ServiceTypes.FirstOrDefaultAsync(s => s.ID == id);

            if (serviceType == null)
                return NotFound();

            return View(serviceType);
        }

        //Post: ServiceTypes/Edit/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ServiceType serviceType)
        {
            if (serviceType == null || id != serviceType.ID)
                return NotFound();

            if (ModelState.IsValid)
            {
                _dbContext.Update(serviceType);
                await _dbContext.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(serviceType);
        }

        //Get: ServiceTypes/Delete/1
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var serviceType = await _dbContext.ServiceTypes.FirstOrDefaultAsync(s => s.ID == id);

            if (serviceType == null)
                return NotFound();

            return View(serviceType);
        }

        //Post: ServiceTypes/Delete/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var serviceTypeToDelete = await _dbContext.ServiceTypes.FirstOrDefaultAsync(s => s.ID == id);
            _dbContext.ServiceTypes.Remove(serviceTypeToDelete);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region ovveride

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dbContext.Dispose();
            }
        }

        #endregion 
    }
}