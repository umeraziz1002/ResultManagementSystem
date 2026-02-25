namespace ResultManagementSystem.Models;
using System.ComponentModel.DataAnnotations;

public class Department
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }
}