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
    public class VaccinationsController : Controller
    {
        private readonly HVKW24_Team2Context _context;

        public VaccinationsController(HVKW24_Team2Context context)
        {
            _context = context;
        }

        // GET: Vaccinations
        public async Task<IActionResult> Index()
        {
              return _context.Vaccinations != null ? 
                          View(await _context.Vaccinations.ToListAsync()) :
                          Problem("Entity set 'HVKW24_Team2Context.Vaccinations'  is null.");
        }

        // GET: Vaccinations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Vaccinations == null)
            {
                return NotFound();
            }

            var vaccination = await _context.Vaccinations
                .FirstOrDefaultAsync(m => m.VaccinationId == id);
            if (vaccination == null)
            {
                return NotFound();
            }

            return View(vaccination);
        }

        // GET: Vaccinations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Vaccinations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VaccinationId,Name")] Vaccination vaccination)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vaccination);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vaccination);
        }

        // GET: Vaccinations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Vaccinations == null)
            {
                return NotFound();
            }

            var vaccination = await _context.Vaccinations.FindAsync(id);
            if (vaccination == null)
            {
                return NotFound();
            }
            return View(vaccination);
        }

        // POST: Vaccinations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VaccinationId,Name")] Vaccination vaccination)
        {
            if (id != vaccination.VaccinationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vaccination);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VaccinationExists(vaccination.VaccinationId))
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
            return View(vaccination);
        }

        // GET: Vaccinations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Vaccinations == null)
            {
                return NotFound();
            }

            var vaccination = await _context.Vaccinations
                .FirstOrDefaultAsync(m => m.VaccinationId == id);
            if (vaccination == null)
            {
                return NotFound();
            }

            return View(vaccination);
        }

        // POST: Vaccinations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Vaccinations == null)
            {
                return Problem("Entity set 'HVKW24_Team2Context.Vaccinations'  is null.");
            }
            var vaccination = await _context.Vaccinations.FindAsync(id);
            if (vaccination != null)
            {
                _context.Vaccinations.Remove(vaccination);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Construction()
        {
            return View();
        }

        private bool VaccinationExists(int id)
        {
          return (_context.Vaccinations?.Any(e => e.VaccinationId == id)).GetValueOrDefault();
        }
    }
}
