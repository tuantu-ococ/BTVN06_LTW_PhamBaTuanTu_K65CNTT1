using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lesson7.Models;

namespace Lesson7.Controllers
{
    public class OrderDetailsController : Controller
    {
        private readonly BookStoreDbContext _context;

        public OrderDetailsController(BookStoreDbContext context)
        {
            _context = context;
        }

        // GET: OrderDetails
        public async Task<IActionResult> Index()
        {
            var details = _context.OrderDetails.Include(o => o.Book).Include(o => o.Order);
            return View(await details.ToListAsync());
        }

        // GET: OrderDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderdetail = await _context.OrderDetails
                .Include(o => o.Book)
                .Include(o => o.Order)
                .FirstOrDefaultAsync(m => m.OrderDetailId == id);
            if (orderdetail == null)
            {
                return NotFound();
            }

            return View(orderdetail);
        }

        // GET: OrderDetails/Create
        public IActionResult Create()
        {
            ViewData["BookId"] = new SelectList(_context.Books, "BookId", "Title");
            ViewData["OrderId"] = new SelectList(_context.OrderBooks, "OrderId", "OrderId");
            return View();
        }

        // POST: OrderDetails/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderDetailId,OrderId,BookId,Quantity,Price")] OrderDetail orderdetail)
        {
            if (ModelState.IsValid)
            {
                _context.Add(orderdetail);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BookId"] = new SelectList(_context.Books, "BookId", "Title", orderdetail.BookId);
            ViewData["OrderId"] = new SelectList(_context.OrderBooks, "OrderId", "OrderId", orderdetail.OrderId);
            return View(orderdetail);
        }

        // GET: OrderDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderdetail = await _context.OrderDetails.FindAsync(id);
            if (orderdetail == null)
            {
                return NotFound();
            }
            ViewData["BookId"] = new SelectList(_context.Books, "BookId", "Title", orderdetail.BookId);
            ViewData["OrderId"] = new SelectList(_context.OrderBooks, "OrderId", "OrderId", orderdetail.OrderId);
            return View(orderdetail);
        }

        // POST: OrderDetails/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderDetailId,OrderId,BookId,Quantity,Price")] OrderDetail orderdetail)
        {
            if (id != orderdetail.OrderDetailId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orderdetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderDetailExists(orderdetail.OrderDetailId))
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
            ViewData["BookId"] = new SelectList(_context.Books, "BookId", "Title", orderdetail.BookId);
            ViewData["OrderId"] = new SelectList(_context.OrderBooks, "OrderId", "OrderId", orderdetail.OrderId);
            return View(orderdetail);
        }

        // GET: OrderDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderdetail = await _context.OrderDetails
                .Include(o => o.Book)
                .Include(o => o.Order)
                .FirstOrDefaultAsync(m => m.OrderDetailId == id);
            if (orderdetail == null)
            {
                return NotFound();
            }

            return View(orderdetail);
        }

        // POST: OrderDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var orderdetail = await _context.OrderDetails.FindAsync(id);
            if (orderdetail != null)
            {
                _context.OrderDetails.Remove(orderdetail);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderDetailExists(int id)
        {
            return _context.OrderDetails.Any(e => e.OrderDetailId == id);
        }
    }
}