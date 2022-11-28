using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Autopodbor_312.Models;

namespace Autopodbor_312.Controllers
{
    public class CarsBrandsModelsController : Controller
    {
        private readonly AutopodborContext _context;

        public CarsBrandsModelsController(AutopodborContext context)
        {
            _context = context;
        }

        // GET: CarsBrandsModels
        public async Task<IActionResult> Index()
        {
            var autopodborContext = _context.CarsBrandsModels.Include(c => c.CarsBrands);
            return View(await autopodborContext.ToListAsync());
        }

        // GET: CarsBrandsModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carsBrandsModel = await _context.CarsBrandsModels
                .Include(c => c.CarsBrands)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (carsBrandsModel == null)
            {
                return NotFound();
            }

            return View(carsBrandsModel);
        }

        // GET: CarsBrandsModels/Create
        public IActionResult Create()
        {
            ViewData["CarsBrandsId"] = new SelectList(_context.CarsBrands, "Id", "Id");
            return View();
        }

        // POST: CarsBrandsModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Model,Price,CarsBrandsId")] CarsBrandsModel carsBrandsModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(carsBrandsModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CarsBrandsId"] = new SelectList(_context.CarsBrands, "Id", "Id", carsBrandsModel.CarsBrandsId);
            return View(carsBrandsModel);
        }

        // GET: CarsBrandsModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carsBrandsModel = await _context.CarsBrandsModels.FindAsync(id);
            if (carsBrandsModel == null)
            {
                return NotFound();
            }
            ViewData["CarsBrandsId"] = new SelectList(_context.CarsBrands, "Id", "Id", carsBrandsModel.CarsBrandsId);
            return View(carsBrandsModel);
        }

        // POST: CarsBrandsModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Model,Price,CarsBrandsId")] CarsBrandsModel carsBrandsModel)
        {
            if (id != carsBrandsModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(carsBrandsModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarsBrandsModelExists(carsBrandsModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CarsBrandsId"] = new SelectList(_context.CarsBrands, "Id", "Id", carsBrandsModel.CarsBrandsId);
            return View(carsBrandsModel);
        }

        // GET: CarsBrandsModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carsBrandsModel = await _context.CarsBrandsModels
                .Include(c => c.CarsBrands)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (carsBrandsModel == null)
            {
                return NotFound();
            }

            return View(carsBrandsModel);
        }

        // POST: CarsBrandsModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var carsBrandsModel = await _context.CarsBrandsModels.FindAsync(id);
            _context.CarsBrandsModels.Remove(carsBrandsModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CarsBrandsModelExists(int id)
        {
            return _context.CarsBrandsModels.Any(e => e.Id == id);
        }
    }
}
