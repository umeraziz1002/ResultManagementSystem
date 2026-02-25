namespace ResultManagementSystem.Models
{
    public class GradePolicy
    {
            public int Id { get; set; }

            public decimal Marks { get; set; }  // exact mark

            public string Grade { get; set; } = string.Empty;

            public decimal GradePoint { get; set; }
        }
    }
