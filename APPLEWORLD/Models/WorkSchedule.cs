namespace AppleWorldShoppingMallEMS.Models;

public class WorkSchedule
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public Employee? Employee { get; set; }
    public DateTime WorkDate { get; set; }
    public int DayNumber { get; set; }
    public string Status { get; set; } = "WORK";
}