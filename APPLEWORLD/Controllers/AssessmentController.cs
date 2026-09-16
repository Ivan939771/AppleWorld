using System.Security.Claims;
using AppleWorldShoppingMallEMS.Data;
using AppleWorldShoppingMallEMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppleWorldShoppingMallEMS.Controllers;

[Authorize(Roles = "Admin")]
public class AssessmentController : Controller
{
    private readonly AppDbContext _db;
    public AssessmentController(AppDbContext db) => _db = db;

    public async Task<IActionResult> Index()
    {
        return View(await _db.Assessments.Include(x => x.Employee).OrderByDescending(x => x.AssessmentDate).ToListAsync());
    }

    [HttpGet]
    public async Task<IActionResult> Create(int? employeeId)
    {
        ViewBag.Employees = await _db.Employees.Where(x => x.IsActive).OrderBy(x => x.FullName).ToListAsync();
        return View(new Assessment { EmployeeId = employeeId ?? 0 });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Assessment model)
    {
        ViewBag.Employees = await _db.Employees.Where(x => x.IsActive).OrderBy(x => x.FullName).ToListAsync();

        if (new[] { model.AttendanceScore, model.PunctualityScore, model.DisciplineScore, model.TeamworkScore, model.WorkQualityScore }.Any(x => x < 0 || x > 100))
        {
            ModelState.AddModelError("", "Scores must be between 0 and 100.");
            return View(model);
        }

        model.OverallScore = (decimal) new[] { model.AttendanceScore, model.PunctualityScore, model.DisciplineScore, model.TeamworkScore, model.WorkQualityScore }.Average();
        model.AssessedBy = User.Identity?.Name ?? "Admin";
        _db.Assessments.Add(model);
        await _db.SaveChangesAsync();
        TempData["Success"] = "Assessment saved.";
        return RedirectToAction(nameof(Index));
    }
}