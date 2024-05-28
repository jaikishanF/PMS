using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Patient_Management_System.Data;
using Patient_Management_System.Models;

namespace Patient_Management_System.Controllers
{
    public class BookingAvailabilitiesController : Controller
    {
        private readonly PMSContext _context;

        public BookingAvailabilitiesController(PMSContext context)
        {
            _context = context;
        }

        // GET: BookingAvailabilities
        public async Task<IActionResult> Index()
        {
            return View(await _context.BookingAvailability.ToListAsync());
        }

        // GET: BookingAvailabilities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookingAvailability = await _context.BookingAvailability
                .FirstOrDefaultAsync(m => m.ID == id);
            if (bookingAvailability == null)
            {
                return NotFound();
            }

            return View(bookingAvailability);
        }

        // GET: BookingAvailabilities/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BookingAvailabilities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Date,StartTime,EndTime,IsAvailable")] BookingAvailability bookingAvailability)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bookingAvailability);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bookingAvailability);
        }

        // GET: BookingAvailabilities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookingAvailability = await _context.BookingAvailability.FindAsync(id);
            if (bookingAvailability == null)
            {
                return NotFound();
            }
            return View(bookingAvailability);
        }

        // POST: BookingAvailabilities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Date,StartTime,EndTime,IsAvailable")] BookingAvailability bookingAvailability)
        {
            if (id != bookingAvailability.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bookingAvailability);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingAvailabilityExists(bookingAvailability.ID))
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
            return View(bookingAvailability);
        }

        // GET: BookingAvailabilities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookingAvailability = await _context.BookingAvailability
                .FirstOrDefaultAsync(m => m.ID == id);
            if (bookingAvailability == null)
            {
                return NotFound();
            }

            return View(bookingAvailability);
        }

        // POST: BookingAvailabilities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bookingAvailability = await _context.BookingAvailability.FindAsync(id);
            if (bookingAvailability != null)
            {
                _context.BookingAvailability.Remove(bookingAvailability);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookingAvailabilityExists(int id)
        {
            return _context.BookingAvailability.Any(e => e.ID == id);
        }
    }
}
