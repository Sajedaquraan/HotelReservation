﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HotelReservation.Models;

namespace HotelReservation.Controllers
{
    public class TestimonialsController : Controller
    {
        private readonly ModelContext _context;

        public TestimonialsController(ModelContext context)
        {
            _context = context;
        }

        // GET: Testimonials
        public async Task<IActionResult> Index()
        {
            var customers = _context.Customers.ToList();
            ViewBag.Customers = customers;
            var id = HttpContext.Session.GetInt32("AdminID");
            var users = _context.Customers.Where(x => x.Customerid == id).SingleOrDefault();
            ViewBag.name = users.Customername;
            ViewBag.image = users.Profileimage;
            ViewBag.email = users.Email;
            var modelContext = _context.Testimonials.Include(t => t.Userloginid2Navigation);
            return View(await modelContext.ToListAsync());
        }

        // GET: Testimonials/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Testimonials == null)
            {
                return NotFound();
            }

            var testimonial = await _context.Testimonials
                .Include(t => t.Userloginid2Navigation)
                .FirstOrDefaultAsync(m => m.Testimonialid == id);
            if (testimonial == null)
            {
                return NotFound();
            }

            return View(testimonial);
        }

        // GET: Testimonials/Create
        public IActionResult Create()
        {
            ViewData["Userloginid2"] = new SelectList(_context.Userlogins, "Userloginid", "Userloginid");

            ViewBag.StateList = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Value = "building", Text = "Building" },
                new SelectListItem { Value = "accepted", Text = "Accepted" },
                new SelectListItem { Value = "rejected", Text = "Rejected" }
            }, "Value", "Text");


            return View();
        }

        // POST: Testimonials/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Testimonialid,Userloginid2,Comments,State")] Testimonial testimonial)
        {
            if (ModelState.IsValid)
            {
                _context.Add(testimonial);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Userloginid2"] = new SelectList(_context.Userlogins, "Userloginid", "Userloginid", testimonial.Userloginid2);

            ViewBag["StateList"] = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Value = "building", Text = "Building" },
                new SelectListItem { Value = "accepted", Text = "Accepted" },
                new SelectListItem { Value = "rejected", Text = "Rejected" }
            }, "Value", "Text", testimonial.State);

            return View(testimonial);
        }

        // GET: Testimonials/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Testimonials == null)
            {
                return NotFound();
            }

            var testimonial = await _context.Testimonials.FindAsync(id);
            if (testimonial == null)
            {
                return NotFound();
            }

            ViewData["Userloginid2"] = new SelectList(_context.Userlogins, "Userloginid", "Userloginid", testimonial.Userloginid2);

            ViewBag.StateList = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Value = "building", Text = "Building" },
                new SelectListItem { Value = "accepted", Text = "Accepted" },
                new SelectListItem { Value = "rejected", Text = "Rejected" }
            }, "Value", "Text", testimonial.State);

            return View(testimonial);
        }

        // POST: Testimonials/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Testimonialid,Userloginid2,Comments,State")] Testimonial testimonial)
        {
            if (id != testimonial.Testimonialid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(testimonial);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TestimonialExists(testimonial.Testimonialid))
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
            ViewData["Userloginid2"] = new SelectList(_context.Userlogins, "Userloginid", "Userloginid", testimonial.Userloginid2);

            ViewBag.StateList = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Value = "building", Text = "Building" },
                new SelectListItem { Value = "accepted", Text = "Accepted" },
                new SelectListItem { Value = "rejected", Text = "Rejected" }
            }, "Value", "Text", testimonial.State);

            return View(testimonial);
        }

        // GET: Testimonials/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Testimonials == null)
            {
                return NotFound();
            }

            var testimonial = await _context.Testimonials
                .Include(t => t.Userloginid2Navigation)
                .FirstOrDefaultAsync(m => m.Testimonialid == id);
            if (testimonial == null)
            {
                return NotFound();
            }

            return View(testimonial);
        }

        // POST: Testimonials/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Testimonials == null)
            {
                return Problem("Entity set 'ModelContext.Testimonials'  is null.");
            }
            var testimonial = await _context.Testimonials.FindAsync(id);
            if (testimonial != null)
            {
                _context.Testimonials.Remove(testimonial);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TestimonialExists(decimal id)
        {
            return (_context.Testimonials?.Any(e => e.Testimonialid == id)).GetValueOrDefault();
        }
    }
}
