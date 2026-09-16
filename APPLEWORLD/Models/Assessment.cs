namespace AppleWorldShoppingMallEMS.Models;

public class Assessment
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public Employee? Employee { get; set; }
    public DateTime AssessmentDate { get; set; } = DateTime.Today;
    public int AttendanceScore { get; set; }
    public int PunctualityScore { get; set; }
    public int DisciplineScore { get; set; }
    public int TeamworkScore { get; set; }
    public int WorkQualityScore { get; set; }
    public decimal OverallScore { get; set; }
    public string? Comments { get; set; }
    public string AssessedBy { get; set; } = "";
}