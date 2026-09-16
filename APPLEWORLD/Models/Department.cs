namespace AppleWorldShoppingMallEMS.Models;

public class Department
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public int RequiredWorkers { get; set; }
    public string GenderRestriction { get; set; } = "Any";
    public ICollection<Employee> Employees { get; set; } = new List<Employee>();
}