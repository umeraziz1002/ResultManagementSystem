namespace ResultManagementSystem.Models
{
    public class Mark
    {

        public int Id { get; set; }

        public int EnrollmentId { get; set; }
        public Enrollment? Enrollment { get; set; }

        public decimal Quiz { get; set; }
        public decimal Assignment { get; set; }
        public decimal Mid { get; set; }
        public decimal Final { get; set; }

        public decimal Total { get; set; }
        public string? Grade { get; set; }
        public decimal GPA { get; set; }
    }


}
