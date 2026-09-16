namespace AppleWorldShoppingMallEMS.Models;

public class AppUser
{
    public int Id { get; set; }
    public int? EmployeeId { get; set; }
    public Employee? Employee { get; set; }
    public string Username { get; set; } = "";
    public string PasswordHash { get; set; } = "";
    public string Role { get; set; } = "Employee";
    public string DisplayName { get; set; } = "";
    public string AccountStatus { get; set; } = "Active";
}