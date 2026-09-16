namespace AppleWorldShoppingMallEMS.Models;

public class Payment
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public Employee? Employee { get; set; }
    public string PaymentPeriod { get; set; } = "";
    public DateTime DueDate { get; set; }
    public decimal Amount { get; set; }
    public string Status { get; set; } = "DUE";
    public DateTime? PaymentDate { get; set; }
}