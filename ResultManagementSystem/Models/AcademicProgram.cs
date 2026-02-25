using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ResultManagementSystem.Data;
namespace ResultManagementSystem.Models;
using System.ComponentModel.DataAnnotations;

public class AcademicProgram
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    public int DurationYears { get; set; }

    public int DepartmentId { get; set; }
    public Department? Department { get; set; }
}