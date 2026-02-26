using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using ResultManagementSystem.Models;

public class TranscriptPdfGenerator
{
    public byte[] Generate(
        ApplicationUser student,
        Semester semester,
        List<Mark> marks,
        decimal semesterGpa,
        decimal cgpa,
        int totalCreditHours,
        string status)
    {
        QuestPDF.Settings.License = LicenseType.Community;

        return Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(40);

                // WATERMARK
                page.Background()
                    .AlignCenter()
                    .AlignMiddle()
                    .Text("OFFICIAL")
                    .FontSize(80)
                    .FontColor(Colors.Grey.Lighten3)
                    .Bold();

                // HEADER
                page.Header().Row(row =>
                {
                    row.ConstantColumn(80)
                        .Image("wwwroot/images/GPGC-LOGO.png");

                    row.RelativeColumn().Column(col =>
                    {
                        col.Item().AlignCenter()
                            .Text("GPGC HARIPUR")
                            .FontSize(18).Bold();

                        col.Item().AlignCenter()
                            .Text("Official Semester Transcript")
                            .FontSize(12);
                    });
                });

                // CONTENT
                page.Content().PaddingVertical(20).Column(column =>
                {
                    column.Spacing(5);

                    column.Item().Text($"Student: {student.FullName}").Bold();
                    column.Item().Text($"Program: {student.Batch?.AcademicProgram?.Name}");
                    column.Item().Text($"Department: {student.Batch?.AcademicProgram?.Department?.Name}");
                    column.Item().Text($"Batch: {student.Batch?.StartYear}");
                    column.Item().Text($"Semester: {semester.Name}");

                    column.Item().PaddingVertical(10);

                    column.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.ConstantColumn(60);
                            columns.ConstantColumn(60);
                            columns.ConstantColumn(50);
                            columns.ConstantColumn(50);
                        });

                        table.Header(header =>
                        {
                            header.Cell().Text("Course Code").Bold();
                            header.Cell().Text("Title").Bold();
                            header.Cell().Text("CH").Bold();
                            header.Cell().Text("Marks").Bold();
                            header.Cell().Text("Grade").Bold();
                            header.Cell().Text("GP").Bold();
                        });

                        foreach (var mark in marks)
                        {
                            table.Cell().Text(mark.Enrollment.CourseOffering.Course.Code);
                            table.Cell().Text(mark.Enrollment.CourseOffering.Course.Title);
                            table.Cell().Text(mark.Enrollment.CourseOffering.Course.CreditHours.ToString());
                            table.Cell().Text(mark.Total.ToString());
                            table.Cell().Text(mark.Grade);
                            table.Cell().Text(mark.GPA.ToString("0.00"));
                        }
                    });

                    column.Item().PaddingTop(15);

                    column.Item().Text($"Total Credit Hours: {totalCreditHours}");
                    column.Item().Text($"Semester GPA: {semesterGpa:0.00}");
                    column.Item().Text($"Cumulative GPA: {cgpa:0.00}");
                    column.Item()
                        .Text($"Status: {status}")
                        .Bold()
                        .FontColor(status == "PASS"
                            ? Colors.Green.Darken2
                            : Colors.Red.Darken2);
                });

                // FOOTER
                page.Footer().Row(row =>
                {
                    row.RelativeColumn().Column(col =>
                    {
                        col.Item().Text("Examination Controller").Bold();
                        col.Item().Image("wwwroot/images/signature.png").FitHeight().FitWidth();
                    });

                    row.RelativeColumn()
                        .AlignRight()
                        .Text($"Generated on {DateTime.Now:dd MMM yyyy}");
                });
            });
        }).GeneratePdf();
    }
}