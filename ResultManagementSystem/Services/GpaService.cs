using Microsoft.EntityFrameworkCore;
using ResultManagementSystem.Data;
using ResultManagementSystem.Models;
namespace ResultManagementSystem.Services
{
    

    public class GpaService
    {
        private readonly ApplicationDbContext _context;

        public GpaService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<(decimal total, string grade, decimal gradePoint)>
            CalculateResultAsync(decimal quiz, decimal assignment, decimal mid, decimal final)
        {
            decimal total = quiz + assignment + mid + final;
            var roundedTotal = Math.Round(total, 0);

            var policy = await _context.GradePolicies
                .FirstOrDefaultAsync(g => g.Marks == roundedTotal);

            if (policy == null)
                return (total, "F", 0);

            return (total, policy.Grade, policy.GradePoint);
        }

        public async Task<decimal> CalculateSemesterGpaAsync(string studentId, int semesterId)
        {
            var marks = await _context.Marks
                .Include(m => m.Enrollment)
                    .ThenInclude(e => e.CourseOffering)
                        .ThenInclude(co => co.Course)
                .Where(m => m.Enrollment!.StudentId == studentId &&
                            m.Enrollment.CourseOffering!.SemesterId == semesterId)
                .ToListAsync();

            if (!marks.Any())
                return 0;

            decimal totalQualityPoints = 0;
            int totalCreditHours = 0;

            foreach (var mark in marks)
            {
                var creditHours = mark.Enrollment!.CourseOffering!.Course!.CreditHours;

                totalQualityPoints += mark.GPA * creditHours;
                totalCreditHours += creditHours;
            }

            return totalCreditHours == 0
                ? 0
                : Math.Round(totalQualityPoints / totalCreditHours, 2);
        }


    }
}
