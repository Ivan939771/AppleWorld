using AppleWorldShoppingMallEMS.Data;
using AppleWorldShoppingMallEMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppleWorldShoppingMallEMS.Controllers;

[Authorize(Roles = "Admin")]
public class EmployeesController : Controller
{
    private readonly AppDbContext _db;
    private readonly IWebHostEnvironment _env;
    private readonly PasswordHasher<AppUser> _hasher = new();

    public EmployeesController(AppDbContext db, IWebHostEnvironment env)
    {
        _db = db; _env = env;
    }

    public async Task<IActionResult> Index(string? search)
    {
        var q = _db.Employees.Include(x => x.Department).AsQueryable();
        if (!string.IsNullOrWhiteSpace(search))
            q = q.Where(x => x.FullName.Contains(search) || x.EmployeeCode.Contains(search) || x.Telephone.Contains(search));
        ViewBag.Search = search;
        return View(await q.OrderBy(x => x.FullName).ToListAsync());
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        ViewBag.Departments = await _db.Departments.OrderBy(x => x.Name).ToListAsync();
        return View(new Employee { RegistrationDate = DateTime.Today });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Employee model, IFormFile? Photo, string password)
    {
        ViewBag.Departments = await _db.Departments.OrderBy(x => x.Name).ToListAsync();

        if (await _db.Employees.AnyAsync(x => x.NationalId == model.NationalId))
            ModelState.AddModelError("NationalId", "This ID number is already registered.");

        if (await _db.Employees.AnyAsync(x => x.Email == model.Email))
            ModelState.AddModelError("Email", "This email is already registered.");

        var dept = await _db.Departments.FindAsync(model.DepartmentId);
        if (dept?.Name == "Blockers" && model.Gender != "Male")
            ModelState.AddModelError("DepartmentId", "Only male employees can be assigned to Blockers.");

        if (!ModelState.IsValid) return View(model);

        model.EmployeeCode = "AWM-" + Guid.NewGuid().ToString("N")[..6].ToUpper();

        if (Photo != null && Photo.Length > 0)
        {
            var ext = Path.GetExtension(Photo.FileName).ToLowerInvariant();
            var allowed = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            if (!allowed.Contains(ext))
            {
                ModelState.AddModelError("", "Only JPG, JPEG, PNG or WEBP images are allowed.");
                return View(model);
            }

            var folder = Path.Combine(_env.WebRootPath, "uploads");
            Directory.CreateDirectory(folder);
            var fileName = Guid.NewGuid().ToString("N") + ext;
            using var stream = new FileStream(Path.Combine(folder, fileName), FileMode.Create);
            await Photo.CopyToAsync(stream);
            model.PhotoPath = "/uploads/" + fileName;
        }

        _db.Employees.Add(model);
        await _db.SaveChangesAsync();

        var user = new AppUser
        {
            EmployeeId = model.Id,
            Username = model.EmployeeCode,
            DisplayName = model.FullName,
            Role = "Employee",
            AccountStatus = "Active"
        };
        user.PasswordHash = _hasher.HashPassword(user, string.IsNullOrWhiteSpace(password) ? "Welcome@123" : password);
        _db.Users.Add(user);

        await CreateNextPayment(model);
        await _db.SaveChangesAsync();

        TempData["Success"] = $"Employee registered. Login username: {model.EmployeeCode}";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var employee = await _db.Employees.FindAsync(id);
        if (employee == null) return NotFound();
        ViewBag.Departments = await _db.Departments.ToListAsync();
        return View(employee);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Employee model, IFormFile? Photo)
    {
        var existing = await _db.Employees.FindAsync(model.Id);
        if (existing == null) return NotFound();

        var dept = await _db.Departments.FindAsync(model.DepartmentId);
        if (dept?.Name == "Blockers" && model.Gender != "Male")
            ModelState.AddModelError("DepartmentId", "Only male employees can be assigned to Blockers.");

        if (!ModelState.IsValid)
        {
            ViewBag.Departments = await _db.Departments.ToListAsync();
            return View(model);
        }

        existing.FullName = model.FullName;
        existing.NationalId = model.NationalId;
        existing.Gender = model.Gender;
        existing.City = model.City;
        existing.Telephone = model.Telephone;
        existing.Email = model.Email;
        existing.RegistrationDate = model.RegistrationDate;
        existing.DepartmentId = model.DepartmentId;
        existing.IsActive = model.IsActive;

        if (Photo != null && Photo.Length > 0)
        {
            var ext = Path.GetExtension(Photo.FileName).ToLowerInvariant();
            var folder = Path.Combine(_env.WebRootPath, "uploads");
            Directory.CreateDirectory(folder);
            var fileName = Guid.NewGuid().ToString("N") + ext;
            using var stream = new FileStream(Path.Combine(folder, fileName), FileMode.Create);
            await Photo.CopyToAsync(stream);
            existing.PhotoPath = "/uploads/" + fileName;
        }

        var user = await _db.Users.FirstOrDefaultAsync(x => x.EmployeeId == model.Id);
        if (user != null)
        {
            user.DisplayName = existing.FullName;
            user.AccountStatus = existing.IsActive ? "Active" : "Inactive";
        }

        await _db.SaveChangesAsync();
        TempData["Success"] = "Employee updated.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Deactivate(int id)
    {
        var e = await _db.Employees.FindAsync(id);
        if (e == null) return NotFound();
        e.IsActive = false;
        var u = await _db.Users.FirstOrDefaultAsync(x => x.EmployeeId == id);
        if (u != null) u.AccountStatus = "Inactive";
        await _db.SaveChangesAsync();
        TempData["Success"] = "Employee deactivated. Their historical records remain available.";
        return RedirectToAction(nameof(Index));
    }

    private async Task CreateNextPayment(Employee employee)
    {
        var due = employee.RegistrationDate.AddMonths(1);
        _db.Payments.Add(new Payment
        {
            EmployeeId = employee.Id,
            PaymentPeriod = due.ToString("MMMM yyyy"),
            DueDate = due,
            Status = "DUE",
            Amount = 0
        });
        await Task.CompletedTask;
    }
}