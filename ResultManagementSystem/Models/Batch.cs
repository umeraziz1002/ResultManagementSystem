namespace ResultManagementSystem.Models;
public class Batch
{
    public int Id { get; set; }

    public int StartYear { get; set; }

    public int AcademicProgramId { get; set; }
    public AcademicProgram? AcademicProgram { get; set; }
}

