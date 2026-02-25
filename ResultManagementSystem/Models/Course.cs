namespace ResultManagementSystem.Models;
using System.ComponentModel.DataAnnotations;

public class Course
{
    public int Id { get; set; }

    public string Code { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    public int CreditHours { get; set; }

    public int AcademicProgramId { get; set; }
    public AcademicProgram? AcademicProgram { get; set; }
}