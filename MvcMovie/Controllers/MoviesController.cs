using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcMovie.Data;
using MvcMovie.Models;

namespace MvcMovie.Controllers;

public class MoviesController : Controller
{
    private readonly MvcMovieContext _context;

    public MoviesController(MvcMovieContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string movieGenre, string searchString)
    {
        if (_context.Movies == null)
        {
            return Problem("Entity set 'MvcMovieContext.Movies' is null.");
        }

        IQueryable<string> genreQuery = from m in _context.Movies
            orderby m.Genre
            select m.Genre!;

        var movies = from m in _context.Movies
            select m;

        if (!string.IsNullOrEmpty(searchString))
        {
            movies = movies.Where(s => s.Title != null && s.Title.Contains(searchString));
        }

        if (!string.IsNullOrEmpty(movieGenre))
        {
            movies = movies.Where(x => x.Genre == movieGenre);
        }

        var movieGenreVM = new MovieGenreViewModel
        {
            Genres = new SelectList(await genreQuery.Distinct().ToListAsync()),
            Movies = await movies.ToListAsync(),
            MovieGenre = movieGenre,
            SearchString = searchString
        };

        return View(movieGenreVM);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var movie = await _context.Movies
            .FirstOrDefaultAsync(m => m.Id == id);

        if (movie == null)
        {
            return NotFound();
        }

        return View(movie);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("Id,Title,ReleaseDate,Genre,Price,Rating,DurationMinutes")] 
        Movie movie)
    {
        if (ModelState.IsValid)
        {
            _context.Add(movie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(movie);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var movie = await _context.Movies.FindAsync(id);
        if (movie == null) return NotFound();

        return View(movie);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, 
        [Bind("Id,Title,ReleaseDate,Genre,Price,Rating,DurationMinutes")] Movie movie)
    {
        if (id != movie.Id) return NotFound();

        if (ModelState.IsValid)
        {
            _context.Update(movie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(movie);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var movie = await _context.Movies
            .FirstOrDefaultAsync(m => m.Id == id);

        if (movie == null)
        {
            return NotFound();
        }

        return View(movie);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var movie = await _context.Movies.FindAsync(id);

        if (movie != null)
            _context.Movies.Remove(movie);

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}