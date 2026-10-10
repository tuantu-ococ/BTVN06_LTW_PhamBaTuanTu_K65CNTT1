
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lesson7.Models;

public class OrderBooksController : Controller
{
    private readonly BookStoreDbContext _context;

    public OrderBooksController(BookStoreDbContext context)
    {
        _context = context;
    }

    // GET: ORDERBOOKS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.OrderBooks.ToListAsync());
    }

    // GET: ORDERBOOKS/Details/5
    public async Task<IActionResult> Details(string? orderid)
    {
        if (orderid == null)
        {
            return NotFound();
        }

        var orderbook = await _context.OrderBooks
            .FirstOrDefaultAsync(m => m.OrderId == orderid);
        if (orderbook == null)
        {
            return NotFound();
        }

        return View(orderbook);
    }

    // GET: ORDERBOOKS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: ORDERBOOKS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("OrderId,OrderDate,AccountId,ReceiveAddress,ReceivePhone,OrderReceive,Note,Status,Account,OrderDetails")] OrderBook orderbook)
    {
        if (ModelState.IsValid)
        {
            _context.Add(orderbook);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(orderbook);
    }

    // GET: ORDERBOOKS/Edit/5
    public async Task<IActionResult> Edit(string? orderid)
    {
        if (orderid == null)
        {
            return NotFound();
        }

        var orderbook = await _context.OrderBooks.FindAsync(orderid);
        if (orderbook == null)
        {
            return NotFound();
        }
        return View(orderbook);
    }

    // POST: ORDERBOOKS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string? orderid, [Bind("OrderId,OrderDate,AccountId,ReceiveAddress,ReceivePhone,OrderReceive,Note,Status,Account,OrderDetails")] OrderBook orderbook)
    {
        if (orderid != orderbook.OrderId)
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
        return View(orderbook);
    }

    // GET: ORDERBOOKS/Delete/5
    public async Task<IActionResult> Delete(string? orderid)
    {
        if (orderid == null)
        {
            return NotFound();
        }

        var orderbook = await _context.OrderBooks
            .FirstOrDefaultAsync(m => m.OrderId == orderid);
        if (orderbook == null)
        {
            return NotFound();
        }

        return View(orderbook);
    }

    // POST: ORDERBOOKS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string? orderid)
    {
        var orderbook = await _context.OrderBooks.FindAsync(orderid);
        if (orderbook != null)
        {
            _context.OrderBooks.Remove(orderbook);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool OrderBookExists(string? orderid)
    {
        return _context.OrderBooks.Any(e => e.OrderId == orderid);
    }
}
