using AppleWorldShoppingMallEMS.Data;
using AppleWorldShoppingMallEMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppleWorldShoppingMallEMS.Controllers;

[Authorize(Roles = "Admin")]
public class ScheduleController : Controller
{
    private readonly AppDbContext _db;
    public ScheduleController(AppDbContext db) => _db = db;

    public async Task<IActionResult> Index(DateTime? start)
    {
        var weekStart = (start ?? DateTime.Today).Date;
        var schedules = await _db.WorkSchedules
            .Include(x => x.Employee).ThenInclude(x => x!.Department)
            .Where(x => x.WorkDate >= weekStart && x.WorkDate < weekStart.AddDays(7))
            .OrderBy(x => x.WorkDate).ThenBy(x => x.Employee!.Department!.Name).ThenBy(x => x.Employee!.FullName)
            .ToListAsync();

        ViewBag.Start = weekStart;
        return View(schedules);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Generate(DateTime? start)
    {
        var weekStart = (start ?? DateTime.Today).Date;
        _db.WorkSchedules.RemoveRange(_db.WorkSchedules.Where(x => x.WorkDate >= weekStart && x.WorkDate < weekStart.AddDays(7)));

        var employees = await _db.Employees.Include(x => x.Department)
            .Where(x => x.IsActive).ToListAsync();

        var rng = Random.Shared;

        foreach (var employee in employees)
        {
            var days = Enumerable.Range(1, 7).OrderBy(_ => rng.Next()).Take(3).OrderBy(x => x).ToList();

            foreach (var day in Enumerable.Range(1, 7))
            {
                _db.WorkSchedules.Add(new WorkSchedule
                {
                    EmployeeId = employee.Id,
                    WorkDate = weekStart.AddDays(day - 1),
                    DayNumber = day,
                    Status = days.Contains(day) ? "WORK" : "OFF"
                });
            }
        }

        await _db.SaveChangesAsync();
        TempData["Success"] = "A new random 7-day schedule was generated. Every active employee has exactly 3 work days.";
        return RedirectToAction(nameof(Index), new { start = weekStart });
    }
}