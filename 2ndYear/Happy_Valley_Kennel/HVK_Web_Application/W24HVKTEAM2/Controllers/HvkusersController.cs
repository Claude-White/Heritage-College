using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using W24HVKTEAM2.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace W24HVKTEAM2.Controllers {

    public class HvkusersController : Controller {
        private readonly HVKW24_Team2Context _context;

        public HvkusersController(HVKW24_Team2Context context) {
            _context = context;
        }

        // GET: Hvkusers
        public async Task<IActionResult> Index() {
            return _context.Hvkusers != null ?
                          View(await _context.Hvkusers.Where((u => u.UserType == "Customer")).ToListAsync()) :
                          Problem("Entity set 'HVKW24_Team2Context.Hvkusers'  is null.");
        }

        [HttpPost]
        public async Task<IActionResult> Index(string? query) {
            if (query != null) {
                var list = await _context.Hvkusers
                    .Where(u => u.UserType == "Customer")
                    .Where(u => u.LastName.Contains(query) ||
                                 u.Email.Contains(query) ||
                                 u.Phone.Contains(query))
                     .ToListAsync();
                return View("Index", list);
            }

            return _context.Hvkusers != null ?
                        View(await _context.Hvkusers.Where(u => u.UserType == "Customer").ToListAsync()) :
                        Problem("Entity set 'HVKW24_Team2Context.Hvkusers'  is null.");
        }

        // GET: Hvkusers/Details/5
        public async Task<IActionResult> Details(int? id) {
            if (id == null || _context.Hvkusers == null) {
                return NotFound();
            }

            var hvkuser = await _context.Hvkusers
                .FirstOrDefaultAsync(m => m.HvkuserId == id);
            if (hvkuser == null) {
                return NotFound();
            }

            return View(hvkuser);
        }

        // GET: Hvkusers/Create
        public IActionResult Create() {
            ViewBag.Provinces = new SelectList(new List<string> { "QC", "ON" });
            return View();
        }

        // POST: Hvkusers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HvkuserId,FirstName,LastName,Email,Password,Street,City,Province,PostalCode,Phone,CellPhone,EmergencyContactFirstName,EmergencyContactLastName,EmergencyContactPhone,UserType")] Hvkuser hvkuser) {
            if (ModelState.IsValid) {
                _context.Add(hvkuser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(hvkuser);
        }

        // GET: Hvkusers/Edit/5
        public async Task<IActionResult> CustomerEdit(int? id) {
            if (id == null || _context.Hvkusers == null) {
                return NotFound();
            }

            var hvkuser = await _context.Hvkusers.FindAsync(id);
            if (hvkuser == null) {
                return NotFound();
            }
            return View(hvkuser);
        }

        // POST: Hvkusers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CustomerEdit(int id, [Bind("HvkuserId,FirstName,LastName,Email,Password,Street,City,Province,PostalCode,Phone,CellPhone,EmergencyContactFirstName,EmergencyContactLastName,EmergencyContactPhone,UserType")] Hvkuser hvkuser) {
            if (id != hvkuser.HvkuserId) {
                return NotFound();
            }

            if (hvkuser.Province == "none") {
                hvkuser.Province = null;
            }
            if (ModelState.IsValid) {
                try {
                    _context.Update(hvkuser);
                    await _context.SaveChangesAsync();
                } catch (DbUpdateConcurrencyException) {
                    if (!HvkuserExists(hvkuser.HvkuserId)) {
                        return NotFound();
                    } else {
                        throw;
                    }
                }
                return RedirectToActionPermanentPreserveMethod("Index", "Home", hvkuser);
            }
            return View(hvkuser);
        }

        // GET: Hvkusers/Edit/5
        public async Task<IActionResult> StaffEdit(int? id) {
            if (id == null || _context.Hvkusers == null) {
                return NotFound();
            }

            var hvkuser = await _context.Hvkusers.FindAsync(id);
            if (hvkuser == null) {
                return NotFound();
            }
            var user = _context.Hvkusers
                    .Include(u => u.Pets)
                    .ThenInclude(p => p.PetReservations)
                    .ThenInclude(pr => pr.Reservation)
                    .FirstOrDefault(u => u.HvkuserId == hvkuser.HvkuserId);
            ViewBag.Pets = user.Pets;
            return View(user);
        }

        // POST: Hvkusers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StaffEdit(int id, [Bind("HvkuserId,FirstName,LastName,Email,Password,Street,City,Province,PostalCode,Phone,CellPhone,EmergencyContactFirstName,EmergencyContactLastName,EmergencyContactPhone,UserType")] Hvkuser hvkuser) {
            if (id != hvkuser.HvkuserId) {
                return NotFound();
            }
            if (hvkuser.Province == "none") {
                hvkuser.Province = null;
            }
            if (ModelState.IsValid) {
                try {
                    _context.Update(hvkuser);
                    await _context.SaveChangesAsync();
                } catch (DbUpdateConcurrencyException) {
                    if (!HvkuserExists(hvkuser.HvkuserId)) {
                        return NotFound();
                    } else {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Hvkusers");
            }
            return View(hvkuser);
        }

        // GET: Hvkusers/Delete/5
        public async Task<IActionResult> Delete(int? id) {
            if (id == null || _context.Hvkusers == null) {
                return NotFound();
            }

            var hvkuser = await _context.Hvkusers
                .FirstOrDefaultAsync(m => m.HvkuserId == id);
            if (hvkuser == null) {
                return NotFound();
            }

            return View(hvkuser);
        }

        // POST: Hvkusers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            if (_context.Hvkusers == null) {
                return Problem("Entity set 'HVKW24_Team2Context.Hvkusers'  is null.");
            }
            var hvkuser = await _context.Hvkusers.FindAsync(id);
            if (hvkuser != null) {
                _context.Hvkusers.Remove(hvkuser);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HvkuserExists(int id) {
            return (_context.Hvkusers?.Any(e => e.HvkuserId == id)).GetValueOrDefault();
        }

        public IActionResult Logout() {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}