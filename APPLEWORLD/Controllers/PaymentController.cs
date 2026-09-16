using AppleWorldShoppingMallEMS.Data;
using AppleWorldShoppingMallEMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppleWorldShoppingMallEMS.Controllers;

[Authorize(Roles = "Admin")]
public class PaymentController : Controller
{
    private readonly AppDbContext _db;
    public PaymentController(AppDbContext db) => _db = db;

    public async Task<IActionResult> Index()
    {
        var today = DateTime.Today;
        var payments = await _db.Payments.Include(x => x.Employee)
            .OrderBy(x => x.DueDate).ToListAsync();

        foreach (var p in payments.Where(x => x.Status != "PAID"))
        {
            if (p.DueDate < today) p.Status = "OVERDUE";
            else if (p.DueDate == today) p.Status = "DUE";
            else if (p.DueDate <= today.AddDays(7)) p.Status = "DUE SOON";
        }
        await _db.SaveChangesAsync();
        return View(payments);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MarkPaid(int id)
    {
        var p = await _db.Payments.FindAsync(id);
        if (p == null) return NotFound();

        p.Status = "PAID";
        p.PaymentDate = DateTime.Today;

        var nextDue = p.DueDate.AddMonths(1);
        _db.Payments.Add(new Payment
        {
            EmployeeId = p.EmployeeId,
            PaymentPeriod = nextDue.ToString("MMMM yyyy"),
            DueDate = nextDue,
            Status = "DUE",
            Amount = p.Amount
        });

        await _db.SaveChangesAsync();
        TempData["Success"] = "Payment marked as paid and next monthly payment created.";
        return RedirectToAction(nameof(Index));
    }
}