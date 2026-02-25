using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ResultManagementSystem.Data;
using ResultManagementSystem.Models;
using ResultManagementSystem.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace ResultManagementSystem.Controllers
{
    [Authorize(Roles = "Examination")]
    public class MarksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly GpaService _gpaService;

        public MarksController(ApplicationDbContext context, GpaService gpaService)
        {
            _context = context;
            _gpaService = gpaService;
        }

        // GET: Marks
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Marks.Include(m => m.Enrollment);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Marks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mark = await _context.Marks
                .Include(m => m.Enrollment)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mark == null)
            {
                return NotFound();
            }

            return View(mark);
        }

        // GET: Marks/Create
        //public IActionResult Create()
        //{
        //    ViewData["EnrollmentId"] = new SelectList(_context.Enrollments, "Id", "Id");
        //    return View();
        //}

        public IActionResult Create()
        {
            var enrollments = _context.Enrollments
                .Include(e => e.Student)
                .Include(e => e.CourseOffering)
                    .ThenInclude(co => co.Course)
                .Include(e => e.CourseOffering)
                    .ThenInclude(co => co.Semester)
                .ToList()
                .Select(e => new
                {
                    e.Id,
                    Display = e.Student!.FullName + " - " +
                              e.CourseOffering!.Course!.Title + " - " +
                              e.CourseOffering!.Semester!.Name
                });

            ViewData["EnrollmentId"] = new SelectList(enrollments, "Id", "Display");

            return View();
        }

        // POST: Marks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(Mark mark)


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EnrollmentId,Quiz,Assignment,Mid,Final,Total,Grade,GPA")] Mark mark)
        {

            if (ModelState.IsValid)
            {

                var result = await _gpaService.CalculateResultAsync(
                    mark.Quiz,
                    mark.Assignment,
                    mark.Mid,
                    mark.Final
                );

                mark.Total = result.total;
                mark.Grade = result.grade;
                mark.GPA = result.gradePoint;

                _context.Add(mark);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var enrollments = _context.Enrollments
    .Include(e => e.Student)
    .Include(e => e.CourseOffering)
        .ThenInclude(co => co.Course)
    .Include(e => e.CourseOffering)
        .ThenInclude(co => co.Semester)
    .ToList()
    .Select(e => new
    {
        e.Id,
        Display = e.Student!.FullName + " - " +
                  e.CourseOffering!.Course!.Title + " - " +
                  e.CourseOffering!.Semester!.Name
    });

            ViewData["EnrollmentId"] = new SelectList(enrollments, "Id", "Display", mark.EnrollmentId);


            return View(mark);
        }

        // GET: Marks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mark = await _context.Marks.FindAsync(id);
            if (mark == null)
            {
                return NotFound();
            }
            ViewData["EnrollmentId"] = new SelectList(_context.Enrollments, "Id", "Id", mark.EnrollmentId);
            return View(mark);
        }

        // POST: Marks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EnrollmentId,Quiz,Assignment,Mid,Final")] Mark mark)
        {
            if (id != mark.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mark);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MarkExists(mark.Id))
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
            ViewData["EnrollmentId"] = new SelectList(_context.Enrollments, "Id", "Id", mark.EnrollmentId);
            return View(mark);
        }

        // GET: Marks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mark = await _context.Marks
                .Include(m => m.Enrollment)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mark == null)
            {
                return NotFound();
            }

            return View(mark);
        }

        // POST: Marks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mark = await _context.Marks.FindAsync(id);
            if (mark != null)
            {
                _context.Marks.Remove(mark);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MarkExists(int id)
        {
            return _context.Marks.Any(e => e.Id == id);
        }
    }
}
