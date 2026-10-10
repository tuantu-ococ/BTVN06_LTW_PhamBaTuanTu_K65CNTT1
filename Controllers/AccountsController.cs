using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lesson7.Models;

namespace Lesson7.Controllers
{
    public class AccountsController : Controller
    {
        private readonly BookStoreDbContext _context;

        public AccountsController(BookStoreDbContext context)
        {
            _context = context;
        }

        // GET: Accounts
        public async Task<IActionResult> Index()
        {
            return View(await _context.Accounts.ToListAsync());
        }

        // GET: Accounts/Details/5
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts
                .FirstOrDefaultAsync(m => m.AccountId == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // GET: Accounts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Accounts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AccountId,Username,Password,FullName,Picture,Email,Address,Phone,IsAdmin,Active")] Account account)
        {
            if (string.IsNullOrEmpty(account.AccountId))
            {
                account.AccountId = Guid.NewGuid().ToString();
            }

            if (ModelState.IsValid)
            {
                _context.Add(account);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(account);
        }

        // GET: Accounts/Edit/5
        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }
            return View(account);
        }

        // POST: Accounts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("AccountId,Username,Password,FullName,Picture,Email,Address,Phone,IsAdmin,Active")] Account account)
        {
            if (id != account.AccountId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(account);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountExists(account.AccountId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(account);
        }

        // GET: Accounts/Delete/5
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts
                .FirstOrDefaultAsync(m => m.AccountId == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // POST: Accounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account != null)
            {
                try
                {
                    _context.Accounts.Remove(account);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    TempData["ErrorMessage"] = "Không thể xóa tài khoản này vì đã có đơn hàng liên kết!";
                    return RedirectToAction(nameof(Delete), new { id = id });
                }
            }

            return RedirectToAction(nameof(Index));
        }

        private bool AccountExists(string id)
        {
            return _context.Accounts.Any(e => e.AccountId == id);
        }
    }
}