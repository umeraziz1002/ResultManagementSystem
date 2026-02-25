using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ResultManagementSystem.Data;
using ResultManagementSystem.Models;

namespace ResultManagementSystem.Controllers
{
    public class CourseOfferingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CourseOfferingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CourseOfferings
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.CourseOfferings.Include(c => c.Course).Include(c => c.Semester).Include(c => c.Teacher);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: CourseOfferings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courseOffering = await _context.CourseOfferings
                .Include(c => c.Course)
                .Include(c => c.Semester)
                .Include(c => c.Teacher)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (courseOffering == null)
            {
                return NotFound();
            }

            return View(courseOffering);
        }

        // GET: CourseOfferings/Create
        public IActionResult Create()
        {
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Title");
            ViewData["SemesterId"] = new SelectList(_context.Semesters, "Id", "SemesterNumber");
            ViewData["TeacherId"] = new SelectList(_context.Users, "Id", "FullName");
            return View();
        }

        // POST: CourseOfferings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CourseId,SemesterId,TeacherId")] CourseOffering courseOffering)
        {
            if (ModelState.IsValid)
            {
                _context.Add(courseOffering);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Id", courseOffering.CourseId);
            ViewData["SemesterId"] = new SelectList(_context.Semesters, "Id", "Id", courseOffering.SemesterId);
            ViewData["TeacherId"] = new SelectList(_context.Users, "Id", "Id", courseOffering.TeacherId);
            return View(courseOffering);
        }

        // GET: CourseOfferings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courseOffering = await _context.CourseOfferings.FindAsync(id);
            if (courseOffering == null)
            {
                return NotFound();
            }
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Id", courseOffering.CourseId);
            ViewData["SemesterId"] = new SelectList(_context.Semesters, "Id", "Id", courseOffering.SemesterId);
            ViewData["TeacherId"] = new SelectList(_context.Users, "Id", "Id", courseOffering.TeacherId);
            return View(courseOffering);
        }

        // POST: CourseOfferings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CourseId,SemesterId,TeacherId")] CourseOffering courseOffering)
        {
            if (id != courseOffering.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(courseOffering);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseOfferingExists(courseOffering.Id))
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
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Id", courseOffering.CourseId);
            ViewData["SemesterId"] = new SelectList(_context.Semesters, "Id", "Id", courseOffering.SemesterId);
            ViewData["TeacherId"] = new SelectList(_context.Users, "Id", "Id", courseOffering.TeacherId);
            return View(courseOffering);
        }

        // GET: CourseOfferings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courseOffering = await _context.CourseOfferings
                .Include(c => c.Course)
                .Include(c => c.Semester)
                .Include(c => c.Teacher)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (courseOffering == null)
            {
                return NotFound();
            }

            return View(courseOffering);
        }

        // POST: CourseOfferings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var courseOffering = await _context.CourseOfferings.FindAsync(id);
            if (courseOffering != null)
            {
                _context.CourseOfferings.Remove(courseOffering);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseOfferingExists(int id)
        {
            return _context.CourseOfferings.Any(e => e.Id == id);
        }
    }
}
