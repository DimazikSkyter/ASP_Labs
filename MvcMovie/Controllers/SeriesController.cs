using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcMovie.Data;
using MvcMovie.Models;

namespace MvcMovie.Controllers;

public class SeriesController : Controller
{
    private readonly MvcMovieContext _context;

    public SeriesController(MvcMovieContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        if (_context.Series == null)
        {
            return Problem("Entity set 'MvcMovieContext.Series' is null.");
        }

        return View(await _context.Series.ToListAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null || _context.Series == null)
        {
            return NotFound();
        }

        var series = await _context.Series
            .FirstOrDefaultAsync(m => m.Id == id);

        if (series == null)
        {
            return NotFound();
        }

        return View(series);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Title,ReleaseDate,Genre,Price,Rating,SeasonsCount")] Series series)
    {
        if (ModelState.IsValid)
        {
            _context.Add(series);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(series);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null || _context.Series == null)
        {
            return NotFound();
        }

        var series = await _context.Series.FindAsync(id);
        if (series == null)
        {
            return NotFound();
        }

        return View(series);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Title,ReleaseDate,Genre,Price,Rating,SeasonsCount")] Series series)
    {
        if (id != series.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(series);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SeriesExists(series.Id))
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

        return View(series);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || _context.Series == null)
        {
            return NotFound();
        }

        var series = await _context.Series
            .FirstOrDefaultAsync(m => m.Id == id);

        if (series == null)
        {
            return NotFound();
        }

        return View(series);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (_context.Series == null)
        {
            return Problem("Entity set 'MvcMovieContext.Series' is null.");
        }

        var series = await _context.Series.FindAsync(id);
        if (series != null)
        {
            _context.Series.Remove(series);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool SeriesExists(int id)
    {
        return (_context.Series?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}