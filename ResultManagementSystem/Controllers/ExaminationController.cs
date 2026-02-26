namespace ResultManagementSystem.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResultManagementSystem.Data;

[Authorize(Roles = "Examination")]
public class ExaminationController : Controller
{
    private readonly ApplicationDbContext _context;

    public ExaminationController(ApplicationDbContext context)
    {
        _context = context;
    }


    public async Task<IActionResult> Dashboard()
    {
        ViewBag.TotalDepartments = await _context.Departments.CountAsync();
        ViewBag.TotalPrograms = await _context.AcademicPrograms.CountAsync();
        ViewBag.TotalBatches = await _context.Batches.CountAsync();
        ViewBag.TotalStudents = await _context.Users
            .CountAsync(u => u.BatchId != null);

        ViewBag.TotalCourses = await _context.Courses.CountAsync();
        ViewBag.TotalMarks = await _context.Marks.CountAsync();

        // Average GPA (All Records)
        var allGpas = await _context.Marks.Select(m => m.GPA).ToListAsync();
        ViewBag.AverageGpa = allGpas.Any()
            ? Math.Round(allGpas.Average(), 2)
            : 0;

        return View();
    }
}