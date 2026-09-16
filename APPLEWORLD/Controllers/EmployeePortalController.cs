using System.Security.Claims;
using AppleWorldShoppingMallEMS.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppleWorldShoppingMallEMS.Controllers;

[Authorize(Roles = "Employee")]
public class EmployeePortalController : Controller
{
    private readonly AppDbContext _db;
    public EmployeePortalController(AppDbContext db) => _db = db;

    private int EmployeeId => int.Parse(User.FindFirstValue("EmployeeId")!);

    public async Task<IActionResult> MySchedule()
    {
        var id = EmployeeId;
        var employee = await _db.Employees.Include(x => x.Department).FirstAsync(x => x.Id == id);
        var schedules = await _db.WorkSchedules.Where(x => x.EmployeeId == id)
            .OrderByDescending(x => x.WorkDate).Take(14).ToListAsync();
        return View((employee, schedules));
    }

    public async Task<IActionResult> Assessments()
    {
        var data = await _db.Assessments.Where(x => x.EmployeeId == EmployeeId)
            .OrderByDescending(x => x.AssessmentDate).ToListAsync();
        return View(data);
    }

    public async Task<IActionResult> Profile()
    {
        var employee = await _db.Employees.Include(x => x.Department).FirstAsync(x => x.Id == EmployeeId);
        return View(employee);
    }
}