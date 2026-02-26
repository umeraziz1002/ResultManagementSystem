using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ResultManagementSystem.Data;
using ResultManagementSystem.Models;
using ResultManagementSystem.Services;



namespace ResultManagementSystem.Controllers
{
    [Authorize(Roles = "Examination")]
    public class MarksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly GpaService _gpaService;

        private readonly TranscriptPdfGenerator _pdfGenerator;

        public MarksController(
            ApplicationDbContext context,
            GpaService gpaService,
            TranscriptPdfGenerator pdfGenerator)
        {
            _context = context;
            _gpaService = gpaService;
            _pdfGenerator = pdfGenerator;
        }


        // GET: Marks
        //public async Task<IActionResult> Index()
        //{
        //    var applicationDbContext = _context.Marks.Include(m => m.Enrollment);
        //    return View(await applicationDbContext.ToListAsync());
        //}
        public async Task<IActionResult> Index()
        {
            var marks = _context.Marks
                .Include(m => m.Enrollment)
                .ThenInclude(e => e.CourseOffering)
                .ThenInclude(co => co.Semester)
                .Include(m => m.Enrollment.Student);

            return View(await marks.ToListAsync());
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

        //public async Task<IActionResult> SemesterResult(string studentId, int semesterId)
        //{
        //    var semesterGpa = await _gpaService
        //        .CalculateSemesterGpaAsync(studentId, semesterId);

        //    var marks = await _context.Marks
        //        .Include(m => m.Enrollment)
        //        .ThenInclude(e => e.CourseOffering)
        //        .ThenInclude(co => co.Course)
        //        .Where(m => m.Enrollment.StudentId == studentId &&
        //                    m.Enrollment.CourseOffering.SemesterId == semesterId)
        //        .ToListAsync();

        //    ViewBag.SemesterGpa = semesterGpa;

        //    return View(marks);
        //}

        public async Task<IActionResult> SemesterResult(string studentId, int semesterId)
        {
            var student = await _context.Users
                .Include(u => u.Batch)
                .ThenInclude(b => b.AcademicProgram)
                .ThenInclude(p => p.Department)
                .FirstOrDefaultAsync(u => u.Id == studentId);

            var semester = await _context.Semesters
                .FirstOrDefaultAsync(s => s.Id == semesterId);

            var marks = await _context.Marks
                .Include(m => m.Enrollment)
                .ThenInclude(e => e.CourseOffering)
                .ThenInclude(co => co.Course)
                .Where(m => m.Enrollment.StudentId == studentId &&
                            m.Enrollment.CourseOffering.SemesterId == semesterId)
                .ToListAsync();

            var semesterGpa = await _gpaService
                .CalculateSemesterGpaAsync(studentId, semesterId);

            var cgpa = await _gpaService.CalculateCgpaAsync(studentId);
            ViewBag.Cgpa = cgpa;

            ViewBag.Student = student;
            ViewBag.Semester = semester;
            ViewBag.SemesterGpa = semesterGpa;

            ViewBag.TotalCreditHours = marks
                .Sum(m => m.Enrollment.CourseOffering.Course.CreditHours);

            ViewBag.Status = marks.Any(m => m.Grade == "F")
                ? "FAIL"
                : "PASS";

            return View(marks);
        }

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

        public IActionResult GenerateResult()
        {
            ViewBag.Students = _context.Users
                .Where(u => u.BatchId != null)
                .ToList();

            ViewBag.Semesters = _context.Semesters.ToList();

            return View();
        }
        public async Task<IActionResult> DownloadTranscript(string studentId, int semesterId)
        {
            var student = await _context.Users
                .Include(u => u.Batch)
                .ThenInclude(b => b.AcademicProgram)
                .ThenInclude(p => p.Department)
                .FirstOrDefaultAsync(u => u.Id == studentId);

            var semester = await _context.Semesters
                .FirstOrDefaultAsync(s => s.Id == semesterId);

            var marks = await _context.Marks
                .Include(m => m.Enrollment)
                .ThenInclude(e => e.CourseOffering)
                .ThenInclude(co => co.Course)
                .Where(m => m.Enrollment.StudentId == studentId &&
                            m.Enrollment.CourseOffering.SemesterId == semesterId)
                .ToListAsync();

            var semesterGpa = await _gpaService.CalculateSemesterGpaAsync(studentId, semesterId);
            var cgpa = await _gpaService.CalculateCgpaAsync(studentId);

            var totalCreditHours = marks.Sum(m => m.Enrollment.CourseOffering.Course.CreditHours);
            var status = marks.Any(m => m.Grade == "F") ? "FAIL" : "PASS";

            var pdfBytes = _pdfGenerator.Generate(
                student,
                semester,
                marks,
                semesterGpa,
                cgpa,
                totalCreditHours,
                status);

            return File(pdfBytes, "application/pdf", "Transcript.pdf");
        }
    }
}
