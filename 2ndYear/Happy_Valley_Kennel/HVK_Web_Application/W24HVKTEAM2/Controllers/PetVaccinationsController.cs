using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using W24HVKTEAM2.Models;

namespace W24HVKTEAM2.Controllers
{
    public class PetVaccinationsController : Controller
    {
        private readonly HVKW24_Team2Context _context;

        public PetVaccinationsController(HVKW24_Team2Context context)
        {
            _context = context;
        }

        // GET: PetVaccinations
        public async Task<IActionResult> Index()
        {
            var hVKW24_Team2Context = _context.PetVaccinations.Include(p => p.Pet).Include(p => p.Vaccination);
            return View(await hVKW24_Team2Context.ToListAsync());
        }

        // GET: PetVaccinations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.PetVaccinations == null)
            {
                return NotFound();
            }

            var petVaccination = await _context.PetVaccinations
                .Include(p => p.Pet)
                .Include(p => p.Vaccination)
                .FirstOrDefaultAsync(m => m.VaccinationId == id);
            if (petVaccination == null)
            {
                return NotFound();
            }

            return View(petVaccination);
        }

        // GET: PetVaccinations/Create
        public IActionResult Create()
        {
            ViewData["PetId"] = new SelectList(_context.Pets, "PetId", "PetId");
            ViewData["VaccinationId"] = new SelectList(_context.Vaccinations, "VaccinationId", "VaccinationId");
            return View();
        }

        // POST: PetVaccinations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ExpiryDate,VaccinationId,PetId,VaccinationChecked")] PetVaccination petVaccination)
        {
            if (ModelState.IsValid)
            {
                _context.Add(petVaccination);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PetId"] = new SelectList(_context.Pets, "PetId", "PetId", petVaccination.PetId);
            ViewData["VaccinationId"] = new SelectList(_context.Vaccinations, "VaccinationId", "VaccinationId", petVaccination.VaccinationId);
            return View(petVaccination);
        }

        // GET: PetVaccinations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.PetVaccinations == null)
            {
                return NotFound();
            }

            var petVaccination = await _context.PetVaccinations.FindAsync(id);
            if (petVaccination == null)
            {
                return NotFound();
            }
            ViewData["PetId"] = new SelectList(_context.Pets, "PetId", "PetId", petVaccination.PetId);
            ViewData["VaccinationId"] = new SelectList(_context.Vaccinations, "VaccinationId", "VaccinationId", petVaccination.VaccinationId);
            return View(petVaccination);
        }

        // POST: PetVaccinations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ExpiryDate,VaccinationId,PetId,VaccinationChecked")] PetVaccination petVaccination)
        {
            if (id != petVaccination.VaccinationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(petVaccination);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PetVaccinationExists(petVaccination.VaccinationId))
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
            ViewData["PetId"] = new SelectList(_context.Pets, "PetId", "PetId", petVaccination.PetId);
            ViewData["VaccinationId"] = new SelectList(_context.Vaccinations, "VaccinationId", "VaccinationId", petVaccination.VaccinationId);
            return View(petVaccination);
        }

        // GET: PetVaccinations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.PetVaccinations == null)
            {
                return NotFound();
            }

            var petVaccination = await _context.PetVaccinations
                .Include(p => p.Pet)
                .Include(p => p.Vaccination)
                .FirstOrDefaultAsync(m => m.VaccinationId == id);
            if (petVaccination == null)
            {
                return NotFound();
            }

            return View(petVaccination);
        }

        // POST: PetVaccinations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.PetVaccinations == null)
            {
                return Problem("Entity set 'HVKW24_Team2Context.PetVaccinations'  is null.");
            }
            var petVaccination = await _context.PetVaccinations.FindAsync(id);
            if (petVaccination != null)
            {
                _context.PetVaccinations.Remove(petVaccination);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PetVaccinationExists(int id)
        {
          return (_context.PetVaccinations?.Any(e => e.VaccinationId == id)).GetValueOrDefault();
        }
    }
}
