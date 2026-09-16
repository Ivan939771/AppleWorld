using AppleWorldShoppingMallEMS.Data;
using AppleWorldShoppingMallEMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppleWorldShoppingMallEMS.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly AppDbContext _db;
    public HomeController(AppDbContext db) => _db = db;

    public async Task<IActionResult> Index()
    {
        var today = DateTime.Today;
        var employees = await _db.Employees.CountAsync();
        var active = await _db.Employees.CountAsync(x => x.IsActive);
        var inactive = employees - active;

        var payments = await _db.Payments
            .Include(x => x.Employee)
            .Where(x => x.Status != "PAID")
            .OrderBy(x => x.DueDate)
            .Take(10)
            .ToListAsync();

        var vm = new DashboardViewModel
        {
            TotalEmployees = employees,
            ActiveEmployees = active,
            InactiveEmployees = inactive,
            DuePayments = payments.Count(x => x.DueDate <= today),
            DueSoonPayments = payments.Count(x => x.DueDate > today && x.DueDate <= today.AddDays(7)),
            PaymentAlerts = payments,
            Departments = await _db.Departments.Include(x => x.Employees).ToListAsync()
        };
        return View(vm);
    }
}