using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lesson7.Models;

namespace Lesson7.Controllers
{
    public class OrderBooksController : Controller
    {
        private readonly BookStoreDbContext _context;

        public OrderBooksController(BookStoreDbContext context)
        {
            _context = context;
        }

        // GET: OrderBooks
        public async Task<IActionResult> Index()
        {
            var orders = _context.OrderBooks.Include(o => o.Account);
            return View(await orders.ToListAsync());
        }

        // GET: OrderBooks/Details/5
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderbook = await _context.OrderBooks
                .Include(o => o.Account)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (orderbook == null)
            {
                return NotFound();
            }

            return View(orderbook);
        }

        // GET: OrderBooks/Create
        public IActionResult Create()
        {
            ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "FullName");
            return View();
        }

        // POST: OrderBooks/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderId,OrderDate,AccountId,ReceiveAddress,ReceivePhone,OrderReceive,Note,Status")] OrderBook orderbook)
        {
            if (ModelState.IsValid)
            {
                _context.Add(orderbook);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "FullName", orderbook.AccountId);
            return View(orderbook);
        }

        // GET: OrderBooks/Edit/5
        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderbook = await _context.OrderBooks.FindAsync(id);
            if (orderbook == null)
            {
                return NotFound();
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "FullName", orderbook.AccountId);
            return View(orderbook);
        }

        // POST: OrderBooks/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("OrderId,OrderDate,AccountId,ReceiveAddress,ReceivePhone,OrderReceive,Note,Status")] OrderBook orderbook)
        {
            if (id != orderbook.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orderbook);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderBookExists(orderbook.OrderId))
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
            ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "FullName", orderbook.AccountId);
            return View(orderbook);
        }

        // GET: OrderBooks/Delete/5
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderbook = await _context.OrderBooks
                .Include(o => o.Account)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (orderbook == null)
            {
                return NotFound();
            }

            return View(orderbook);
        }

        // POST: OrderBooks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var orderbook = await _context.OrderBooks.FindAsync(id);
            if (orderbook != null)
            {
                try
                {
                    _context.OrderBooks.Remove(orderbook);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    TempData["ErrorMessage"] = "Không thể xóa đơn hàng vì vẫn còn chi tiết hóa đơn (OrderDetail)! Hãy xóa chi tiết trước.";
                    return RedirectToAction(nameof(Delete), new { id = id });
                }
            }

            return RedirectToAction(nameof(Index));
        }

        private bool OrderBookExists(string id)
        {
            return _context.OrderBooks.Any(e => e.OrderId == id);
        }
    }
}