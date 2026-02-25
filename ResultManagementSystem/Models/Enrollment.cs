namespace ResultManagementSystem.Models
{
    public class Enrollment
    {
      
  
        public int Id { get; set; }

        public string StudentId { get; set; } = string.Empty;
        public ApplicationUser? Student { get; set; }

        public int CourseOfferingId { get; set; }
        public CourseOffering? CourseOffering { get; set; }

        public bool IsCompleted { get; set; } = false;
    }


}
