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

    // Dashboard
    public async Task<IActionResult> Dashboard()
    {
        ViewBag.TotalDepartments = await _context.Departments.CountAsync();
        ViewBag.TotalPrograms = await _context.AcademicPrograms.CountAsync();
        ViewBag.TotalBatches = await _context.Batches.CountAsync();
        ViewBag.TotalCourses = await _context.Courses.CountAsync();
        ViewBag.TotalSemesters = await _context.Semesters.CountAsync();
        ViewBag.TotalEnrollments = await _context.Enrollments.CountAsync();
        ViewBag.TotalMarks = await _context.Marks.CountAsync();

        return View();
    }
}