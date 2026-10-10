
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lesson7.Models;

public class PublishersController : Controller
{
    private readonly BookStoreDbContext _context;

    public PublishersController(BookStoreDbContext context)
    {
        _context = context;
    }

    // GET: PUBLISHERS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.Publishers.ToListAsync());
    }

    // GET: PUBLISHERS/Details/5
    public async Task<IActionResult> Details(int? publisherid)
    {
        if (publisherid == null)
        {
            return NotFound();
        }

        var publisher = await _context.Publishers
            .FirstOrDefaultAsync(m => m.PublisherId == publisherid);
        if (publisher == null)
        {
            return NotFound();
        }

        return View(publisher);
    }

    // GET: PUBLISHERS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: PUBLISHERS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("PublisherId,PublisherName,Phone,Address,Books")] Publisher publisher)
    {
        if (ModelState.IsValid)
        {
            _context.Add(publisher);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(publisher);
    }

    // GET: PUBLISHERS/Edit/5
    public async Task<IActionResult> Edit(int? publisherid)
    {
        if (publisherid == null)
        {
            return NotFound();
        }

        var publisher = await _context.Publishers.FindAsync(publisherid);
        if (publisher == null)
        {
            return NotFound();
        }
        return View(publisher);
    }

    // POST: PUBLISHERS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? publisherid, [Bind("PublisherId,PublisherName,Phone,Address,Books")] Publisher publisher)
    {
        if (publisherid != publisher.PublisherId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(publisher);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PublisherExists(publisher.PublisherId))
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
        return View(publisher);
    }

    // GET: PUBLISHERS/Delete/5
    public async Task<IActionResult> Delete(int? publisherid)
    {
        if (publisherid == null)
        {
            return NotFound();
        }

        var publisher = await _context.Publishers
            .FirstOrDefaultAsync(m => m.PublisherId == publisherid);
        if (publisher == null)
        {
            return NotFound();
        }

        return View(publisher);
    }

    // POST: PUBLISHERS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? publisherid)
    {
        var publisher = await _context.Publishers.FindAsync(publisherid);
        if (publisher != null)
        {
            _context.Publishers.Remove(publisher);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool PublisherExists(int? publisherid)
    {
        return _context.Publishers.Any(e => e.PublisherId == publisherid);
    }
}
