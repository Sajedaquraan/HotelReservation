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
    public class UserloginsController : Controller
    {
        private readonly ModelContext _context;

        public UserloginsController(ModelContext context)
        {
            _context = context;
        }

        // GET: Userlogins
        public async Task<IActionResult> Index()
        {
            var customers = _context.Customers.ToList();
            ViewBag.Customers = customers;
            var id = HttpContext.Session.GetInt32("AdminID");
            var users = _context.Customers.Where(x => x.Customerid == id).SingleOrDefault();
            ViewBag.name = users.Customername;
            ViewBag.image = users.Profileimage;
            ViewBag.email = users.Email;
            var modelContext = _context.Userlogins.Include(u => u.Customer).Include(u => u.Role);
            return View(await modelContext.ToListAsync());
        }

        // GET: Userlogins/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Userlogins == null)
            {
                return NotFound();
            }

            var userlogin = await _context.Userlogins
                .Include(u => u.Customer)
                .Include(u => u.Role)
                .FirstOrDefaultAsync(m => m.Userloginid == id);
            if (userlogin == null)
            {
                return NotFound();
            }


            return View(userlogin);
        }

        // GET: Userlogins/Create
        public IActionResult Create()
        {
            ViewData["Customerid"] = new SelectList(_context.Customers, "Customerid", "Customerid");
            ViewData["Roleid"] = new SelectList(_context.Roles, "Roleid", "Roleid");
            return View();
        }

        // POST: Userlogins/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Userloginid,Email,Password,Customerid,Roleid")] Userlogin userlogin)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userlogin);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Customerid"] = new SelectList(_context.Customers, "Customerid", "Customerid", userlogin.Customerid);
            ViewData["Roleid"] = new SelectList(_context.Roles, "Roleid", "Roleid", userlogin.Roleid);
            return View(userlogin);
        }

        // GET: Userlogins/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Userlogins == null)
            {
                return NotFound();
            }

            var userlogin = await _context.Userlogins.FindAsync(id);
            if (userlogin == null)
            {
                return NotFound();
            }
            ViewData["Customerid"] = new SelectList(_context.Customers, "Customerid", "Customerid", userlogin.Customerid);
            ViewData["Roleid"] = new SelectList(_context.Roles, "Roleid", "Roleid", userlogin.Roleid);
            return View(userlogin);
        }

        // POST: Userlogins/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Userloginid,Email,Password,Customerid,Roleid")] Userlogin userlogin)
        {
            if (id != userlogin.Userloginid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userlogin);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserloginExists(userlogin.Userloginid))
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
            ViewData["Customerid"] = new SelectList(_context.Customers, "Customerid", "Customerid", userlogin.Customerid);
            ViewData["Roleid"] = new SelectList(_context.Roles, "Roleid", "Roleid", userlogin.Roleid);
            return View(userlogin);
        }

        // GET: Userlogins/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Userlogins == null)
            {
                return NotFound();
            }

            var userlogin = await _context.Userlogins
                .Include(u => u.Customer)
                .Include(u => u.Role)
                .FirstOrDefaultAsync(m => m.Userloginid == id);
            if (userlogin == null)
            {
                return NotFound();
            }

            return View(userlogin);
        }

        // POST: Userlogins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Userlogins == null)
            {
                return Problem("Entity set 'ModelContext.Userlogins'  is null.");
            }
            var userlogin = await _context.Userlogins.FindAsync(id);
            if (userlogin != null)
            {
                _context.Userlogins.Remove(userlogin);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserloginExists(decimal id)
        {
          return (_context.Userlogins?.Any(e => e.Userloginid == id)).GetValueOrDefault();
        }
    }
}
