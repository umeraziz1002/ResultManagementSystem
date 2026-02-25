using Microsoft.AspNetCore.Identity;
using ResultManagementSystem.Models;

namespace ResultManagementSystem.Data;

public static class DbInitializer
{
    public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
        string[] roles = { "Admin", "Teacher", "Student", "HOD", "Examination" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }

        // Create Default Admin
        string adminEmail = "admin@college.com";
        string adminPassword = "Admin@123";

        var adminUser = await userManager.FindByEmailAsync(adminEmail);

        if (adminUser == null)
        {
            var user = new ApplicationUser
            {
                FullName = "Admin User",
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };

            await userManager.CreateAsync(user, adminPassword);
            await userManager.AddToRoleAsync(user, "Admin");
        }

        // Create Default Teacher
        string teacherEmail = "teacher@college.com";
        string teacherPassword = "Teacher@123";

        var teacherUser = await userManager.FindByEmailAsync(teacherEmail);

        if (teacherUser == null)
        {
            var teacher = new ApplicationUser
            {
                UserName = teacherEmail,
                Email = teacherEmail,
                EmailConfirmed = true,
                FullName = "Dr. Ahmed",
                CNIC = "11111-1111111-1"
            };

            await userManager.CreateAsync(teacher, teacherPassword);
            await userManager.AddToRoleAsync(teacher, "Teacher");
        }

        // Create Default Student
        string studentEmail = "student@college.com";
        string studentPassword = "Student@123";

        var studentUser = await userManager.FindByEmailAsync(studentEmail);

        if (studentUser == null)
        {
            var student = new ApplicationUser
            {
                UserName = studentEmail,
                Email = studentEmail,
                EmailConfirmed = true,
                FullName = "Ali Khan",
                CNIC = "22222-2222222-2"
            };

            await userManager.CreateAsync(student, studentPassword);
            await userManager.AddToRoleAsync(student, "Student");
        }


        string examEmail = "exam@college.com";
        string examPassword = "Exam@123";

        var examUser = await userManager.FindByEmailAsync(examEmail);

        if (examUser == null)
        {
            var examination = new ApplicationUser
            {
                UserName = examEmail,
                Email = examEmail,
                EmailConfirmed = true,
                FullName = "Examination Officer",
                CNIC = "33333-3333333-3"
            };

            await userManager.CreateAsync(examination, examPassword);
            await userManager.AddToRoleAsync(examination, "Examination");
        }

        if (!context.GradePolicies.Any())
        {
            var policies = new List<GradePolicy>();

            // Below 50 = F
            for (int i = 0; i < 50; i++)
            {
                policies.Add(new GradePolicy
                {
                    Marks = i,
                    Grade = "F",
                    GradePoint = 0.00m
                });
            }

            // 50–54 D
            policies.AddRange(new[]
            {
        new GradePolicy { Marks = 50, Grade = "D", GradePoint = 1.00m },
        new GradePolicy { Marks = 51, Grade = "D", GradePoint = 1.08m },
        new GradePolicy { Marks = 52, Grade = "D", GradePoint = 1.17m },
        new GradePolicy { Marks = 53, Grade = "D", GradePoint = 1.25m },
        new GradePolicy { Marks = 54, Grade = "D+", GradePoint = 1.33m },
        new GradePolicy { Marks = 55, Grade = "D+", GradePoint = 1.42m },
        new GradePolicy { Marks = 56, Grade = "D+", GradePoint = 1.50m },
        new GradePolicy { Marks = 57, Grade = "D+", GradePoint = 1.58m },
        new GradePolicy { Marks = 58, Grade = "C-", GradePoint = 1.67m },
        new GradePolicy { Marks = 59, Grade = "C-", GradePoint = 1.75m },
        new GradePolicy { Marks = 60, Grade = "C-", GradePoint = 1.83m },
        new GradePolicy { Marks = 61, Grade = "C", GradePoint = 1.92m },
        new GradePolicy { Marks = 62, Grade = "C", GradePoint = 2.00m },
        new GradePolicy { Marks = 63, Grade = "C", GradePoint = 2.08m },
        new GradePolicy { Marks = 64, Grade = "C+", GradePoint = 2.17m },
        new GradePolicy { Marks = 65, Grade = "C+", GradePoint = 2.25m },
        new GradePolicy { Marks = 66, Grade = "C+", GradePoint = 2.33m },
        new GradePolicy { Marks = 67, Grade = "C+", GradePoint = 2.42m },
        new GradePolicy { Marks = 68, Grade = "B-", GradePoint = 2.50m },
        new GradePolicy { Marks = 69, Grade = "B-", GradePoint = 2.58m },
        new GradePolicy { Marks = 70, Grade = "B-", GradePoint = 2.67m },
        new GradePolicy { Marks = 71, Grade = "B", GradePoint = 2.75m },
        new GradePolicy { Marks = 72, Grade = "B", GradePoint = 2.83m },
        new GradePolicy { Marks = 73, Grade = "B", GradePoint = 2.92m },
        new GradePolicy { Marks = 74, Grade = "B", GradePoint = 3.00m },
        new GradePolicy { Marks = 75, Grade = "B+", GradePoint = 3.08m },
        new GradePolicy { Marks = 76, Grade = "B+", GradePoint = 3.17m },
        new GradePolicy { Marks = 77, Grade = "B+", GradePoint = 3.25m },
        new GradePolicy { Marks = 78, Grade = "B+", GradePoint = 3.33m },
        new GradePolicy { Marks = 79, Grade = "B+", GradePoint = 3.42m },
        new GradePolicy { Marks = 80, Grade = "A-", GradePoint = 3.50m },
        new GradePolicy { Marks = 81, Grade = "A-", GradePoint = 3.60m },
        new GradePolicy { Marks = 82, Grade = "A-", GradePoint = 3.70m },
        new GradePolicy { Marks = 83, Grade = "A-", GradePoint = 3.80m },
        new GradePolicy { Marks = 84, Grade = "A-", GradePoint = 3.90m },
    });

            // 85–100 = A (4.00)
            for (int i = 85; i <= 100; i++)
            {
                policies.Add(new GradePolicy
                {
                    Marks = i,
                    Grade = "A",
                    GradePoint = 4.00m
                });
            }

            context.GradePolicies.AddRange(policies);
            await context.SaveChangesAsync();
        }


    }
}