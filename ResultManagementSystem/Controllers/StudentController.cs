using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResultManagementSystem.Data;
using ResultManagementSystem.Models;

public class StudentsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public StudentsController(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // LIST STUDENTS
    public async Task<IActionResult> Index()
    {
        var students = await _context.Users
            .Include(u => u.Batch)
            .ThenInclude(b => b.AcademicProgram)
            .Where(u => u.BatchId != null)
            .ToListAsync();

        return View(students);
    }

    // CREATE (GET)
    public IActionResult Create()
    {
        ViewBag.Batches = _context.Batches
            .Include(b => b.AcademicProgram)
            .ToList();

        return View();
    }

    // CREATE (POST)
    [HttpPost]
    public async Task<IActionResult> Create(
        string fullName,
        string email,
        string password,
        int batchId)
    {
        var user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            FullName = fullName,
            BatchId = batchId
        };

        var result = await _userManager.CreateAsync(user, password);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, "Student");
            return RedirectToAction("Index");
        }

        ViewBag.Batches = _context.Batches
            .Include(b => b.AcademicProgram)
            .ToList();

        return View();
    }
}