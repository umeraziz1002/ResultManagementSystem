namespace ResultManagementSystem.Models
{
    public class CourseOffering
    {
       

        public int Id { get; set; }

        public int CourseId { get; set; }
        public Course? Course { get; set; }

        public int SemesterId { get; set; }
        public Semester? Semester { get; set; }

        public string TeacherId { get; set; } = string.Empty;
        public ApplicationUser? Teacher { get; set; }
    }


}
