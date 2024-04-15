using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using W24HVKTEAM2.Models;

namespace W24HVKTEAM2.Controllers {

    public class PetsController : Controller {
        private readonly HVKW24_Team2Context _context;

        public PetsController(HVKW24_Team2Context context) {
            _context = context;
        }

        // GET: Pets
        public async Task<IActionResult> Index() {
            var hVKW24_Team2Context = _context.Pets.Include(p => p.Hvkuser);
            return View(await hVKW24_Team2Context.ToListAsync());
        }

        // GET: Pets/Details/5
        public async Task<IActionResult> Details(int? id) {
            if (id == null || _context.Pets == null) {
                return NotFound();
            }

            var pet = await _context.Pets
                .Include(p => p.Hvkuser)
                .FirstOrDefaultAsync(m => m.PetId == id);
            if (pet == null) {
                return NotFound();
            }

            return View(pet);
        }

        // GET: Pets/Create
        public IActionResult Create() {
            ViewData["HvkuserId"] = new SelectList(_context.Hvkusers, "HvkuserId", "FirstName");
            return View();
        }

        // POST: Pets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PetId,Name,Gender,Breed,Birthyear,HvkuserId,DogSize,Climber,Barker,SpecialNotes,Sterilized")] Pet pet) {
            if (ModelState.IsValid) {
                _context.Add(pet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["HvkuserId"] = new SelectList(_context.Hvkusers, "HvkuserId", "FirstName", pet.HvkuserId);
            return View(pet);
        }

        // GET: Pets/Edit/5
        public async Task<IActionResult> CustomerEdit(int? id) {
            if (id == null || _context.Pets == null) {
                return NotFound();
            }

            var pet = _context.Pets.Include(p => p.PetVaccinations).ThenInclude(pv => pv.Vaccination).FirstOrDefault(p => p.PetId == id);
            if (pet == null) {
                return NotFound();
            }
            ViewData["HvkuserId"] = new SelectList(_context.Hvkusers, "HvkuserId", "FirstName", pet.HvkuserId);
            return View("CustomerEdit", pet);
        }

        // POST: Pets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CustomerEdit(int id, [Bind("PetId,Name,Gender,Breed,Birthyear,HvkuserId,DogSize,Climber,Barker,SpecialNotes,Sterilized")] Pet pet) {
            if (id != pet.PetId) {
                return NotFound();
            }

            pet.Hvkuser = _context.Hvkusers.FirstOrDefault(u => u.HvkuserId == pet.HvkuserId);
            pet.Gender = pet.Gender.Equals("Male") ? "M" : "F";
            pet.DogSize = pet.DogSize.Equals("Small") ? "S" : pet.DogSize.Equals("Medium") ? "M" : "L";
            ModelState.Remove("Hvkuser");

            if (ModelState.IsValid) {
                try {
                    _context.Update(pet);
                    await _context.SaveChangesAsync();
                } catch (DbUpdateConcurrencyException) {
                    if (!PetExists(pet.PetId)) {
                        return NotFound();
                    } else {
                        throw;
                    }
                }
                var tempPet = _context.Pets.FirstOrDefault(p => p.PetId == id);
                var hvkuser = _context.Hvkusers.FirstOrDefault(u => u.HvkuserId == tempPet.HvkuserId);
                return RedirectToActionPermanentPreserveMethod("Index", "Home", hvkuser);
            }
            ViewData["HvkuserId"] = new SelectList(_context.Hvkusers, "HvkuserId", "FirstName", pet.HvkuserId);
            return RedirectToActionPermanent("CustomerEdit", "Pets", pet.PetId);
        }

        public async Task<IActionResult> StaffEdit(int? id) {
            if (id == null || _context.Pets == null) {
                return NotFound();
            }

            var pet = _context.Pets.Include(p => p.PetVaccinations).ThenInclude(pv => pv.Vaccination).FirstOrDefault(p => p.PetId == id);
            if (pet == null) {
                return NotFound();
            }
            ViewData["HvkuserId"] = new SelectList(_context.Hvkusers, "HvkuserId", "FirstName", pet.HvkuserId);
            return View("StaffEdit", pet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StaffEdit(int id, [Bind("PetId,Name,Gender,Breed,Birthyear,HvkuserId,DogSize,Climber,Barker,SpecialNotes,Sterilized")] Pet pet) {
            if (id != pet.PetId) {
                return NotFound();
            }

            pet.Hvkuser = _context.Hvkusers.FirstOrDefault(u => u.HvkuserId == pet.HvkuserId);
            pet.Gender = pet.Gender.Equals("Male") ? "M" : "F";
            pet.DogSize = pet.DogSize.Equals("Small") ? "S" : pet.DogSize.Equals("Medium") ? "M" : "L";
            ModelState.Remove("Hvkuser");

            if (ModelState.IsValid) {
                try {
                    _context.Update(pet);
                    await _context.SaveChangesAsync();
                } catch (DbUpdateConcurrencyException) {
                    if (!PetExists(pet.PetId)) {
                        return NotFound();
                    } else {
                        throw;
                    }
                }
                var tempPet = _context.Pets.FirstOrDefault(p => p.PetId == id);
                var hvkuser = _context.Hvkusers.FirstOrDefault(u => u.HvkuserId == tempPet.HvkuserId);
                return RedirectToAction("StaffEdit", "Hvkusers", new { id = pet.HvkuserId });
            }
            ViewData["HvkuserId"] = new SelectList(_context.Hvkusers, "HvkuserId", "FirstName", pet.HvkuserId);
            return RedirectToActionPermanent("StaffEdit", "Pets", pet.PetId);
        }

        // GET: Pets/Delete/5
        public async Task<IActionResult> Delete(int? id) {
            if (id == null || _context.Pets == null) {
                return NotFound();
            }

            var pet = await _context.Pets
                .Include(p => p.Hvkuser)
                .FirstOrDefaultAsync(m => m.PetId == id);
            if (pet == null) {
                return NotFound();
            }

            return View(pet);
        }

        // POST: Pets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            if (_context.Pets == null) {
                return Problem("Entity set 'HVKW24_Team2Context.Pets'  is null.");
            }
            var pet = await _context.Pets.FindAsync(id);
            if (pet != null) {
                _context.Pets.Remove(pet);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PetExists(int id) {
            return (_context.Pets?.Any(e => e.PetId == id)).GetValueOrDefault();
        }

        public IActionResult ManagePet() {
            var petVaccinations = from pet in _context.Pets.Include(p => p.PetVaccinations).ThenInclude(pv => pv.Vaccination) where pet.PetId == 14 select pet;

            return View(petVaccinations);
        }
    }
}