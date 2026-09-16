namespace AppleWorldShoppingMallEMS.Models;

public class DashboardViewModel
{
    public int TotalEmployees { get; set; }
    public int ActiveEmployees { get; set; }
    public int InactiveEmployees { get; set; }
    public int DuePayments { get; set; }
    public int DueSoonPayments { get; set; }
    public List<Department> Departments { get; set; } = new();
    public List<Payment> PaymentAlerts { get; set; } = new();
}