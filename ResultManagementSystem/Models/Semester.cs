using ResultManagementSystem.Models;

public class Semester
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;   // Fall 2025

    public int SemesterNumber { get; set; }           // 1–8

    public int BatchId { get; set; }
    public Batch? Batch { get; set; }

    public bool IsActive { get; set; } = true;
}