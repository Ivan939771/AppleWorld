using System.ComponentModel.DataAnnotations;

namespace AppleWorldShoppingMallEMS.Models;

public class Employee
{
    public int Id { get; set; }

    [Required, StringLength(30)]
    public string EmployeeCode { get; set; } = "";

    [Required, StringLength(120)]
    public string FullName { get; set; } = "";

    [Required, StringLength(40)]
    public string NationalId { get; set; } = "";

    [Required]
    public string Gender { get; set; } = "Male";

    [Required, StringLength(80)]
    public string City { get; set; } = "";

    [Required, StringLength(30)]
    public string Telephone { get; set; } = "";

    [Required, EmailAddress, StringLength(120)]
    public string Email { get; set; } = "";

    public DateTime RegistrationDate { get; set; } = DateTime.Today;

    public string? PhotoPath { get; set; }

    public bool IsActive { get; set; } = true;

    public int DepartmentId { get; set; }
    public Department? Department { get; set; }

    public AppUser? User { get; set; }
    public ICollection<WorkSchedule> Schedules { get; set; } = new List<WorkSchedule>();
    public ICollection<Assessment> Assessments { get; set; } = new List<Assessment>();
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}