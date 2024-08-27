using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HotelReservation.Models;

namespace HotelReservation.Controllers
{
    public class PaymentroomsController : Controller
    {
        private readonly ModelContext _context;

        public PaymentroomsController(ModelContext context)
        {
            _context = context;
        }

        // GET: Paymentrooms
        public async Task<IActionResult> Index()
        {
            var customers = _context.Customers.ToList();
            ViewBag.Customers = customers;
            var id = HttpContext.Session.GetInt32("AdminID");
            var users = _context.Customers.Where(x => x.Customerid == id).SingleOrDefault();
            ViewBag.name = users.Customername;
            ViewBag.image = users.Profileimage;
            ViewBag.email = users.Email;
            var modelContext = _context.Paymentrooms.Include(p => p.Bank).Include(p => p.Reservation);
            return View(await modelContext.ToListAsync());
        }

        // GET: Paymentrooms/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Paymentrooms == null)
            {
                return NotFound();
            }

            var paymentroom = await _context.Paymentrooms
                .Include(p => p.Bank)
                .Include(p => p.Reservation)
                .FirstOrDefaultAsync(m => m.Paymentid == id);
            if (paymentroom == null)
            {
                return NotFound();
            }

            return View(paymentroom);
        }

        // GET: Paymentrooms/Create
        public IActionResult Create()
        {
            ViewData["Bankid"] = new SelectList(_context.Banks, "Bankid", "Bankid");
            ViewData["Reservationid"] = new SelectList(_context.Reservationrooms, "Reservationid", "Reservationid");
            return View();
        }

        // POST: Paymentrooms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Paymentid,Reservationid,Amountpaid,Paymentdate,Bankid")] Paymentroom paymentroom)
        {
            if (ModelState.IsValid)
            {
                _context.Add(paymentroom);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Bankid"] = new SelectList(_context.Banks, "Bankid", "Bankid", paymentroom.Bankid);
            ViewData["Reservationid"] = new SelectList(_context.Reservationrooms, "Reservationid", "Reservationid", paymentroom.Reservationid);
            return View(paymentroom);
        }

        // GET: Paymentrooms/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Paymentrooms == null)
            {
                return NotFound();
            }

            var paymentroom = await _context.Paymentrooms.FindAsync(id);
            if (paymentroom == null)
            {
                return NotFound();
            }
            ViewData["Bankid"] = new SelectList(_context.Banks, "Bankid", "Bankid", paymentroom.Bankid);
            ViewData["Reservationid"] = new SelectList(_context.Reservationrooms, "Reservationid", "Reservationid", paymentroom.Reservationid);
            return View(paymentroom);
        }

        // POST: Paymentrooms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Paymentid,Reservationid,Amountpaid,Paymentdate,Bankid")] Paymentroom paymentroom)
        {
            if (id != paymentroom.Paymentid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(paymentroom);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PaymentroomExists(paymentroom.Paymentid))
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
            ViewData["Bankid"] = new SelectList(_context.Banks, "Bankid", "Bankid", paymentroom.Bankid);
            ViewData["Reservationid"] = new SelectList(_context.Reservationrooms, "Reservationid", "Reservationid", paymentroom.Reservationid);
            return View(paymentroom);
        }

        // GET: Paymentrooms/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Paymentrooms == null)
            {
                return NotFound();
            }

            var paymentroom = await _context.Paymentrooms
                .Include(p => p.Bank)
                .Include(p => p.Reservation)
                .FirstOrDefaultAsync(m => m.Paymentid == id);
            if (paymentroom == null)
            {
                return NotFound();
            }

            return View(paymentroom);
        }

        // POST: Paymentrooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Paymentrooms == null)
            {
                return Problem("Entity set 'ModelContext.Paymentrooms'  is null.");
            }
            var paymentroom = await _context.Paymentrooms.FindAsync(id);
            if (paymentroom != null)
            {
                _context.Paymentrooms.Remove(paymentroom);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PaymentroomExists(decimal id)
        {
          return (_context.Paymentrooms?.Any(e => e.Paymentid == id)).GetValueOrDefault();
        }
    }
}
