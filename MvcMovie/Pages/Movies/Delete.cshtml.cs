using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MvcMovie.Data;
using MvcMovie.Models;

namespace MvcMovie.Pages.Movies;

public class DeleteModel : PageModel
{
    private readonly MvcMovieContext _context;
    private readonly ILogger<DeleteModel> _logger;

    public DeleteModel(MvcMovieContext context, ILogger<DeleteModel> logger)
    {
        _context = context;
        _logger = logger;
    }

    [BindProperty]
    public Movie Movie { get; set; } = default!;

    public string? ErrorMessage { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id, bool? saveChangesError = false)
    {
        if (id == null) return NotFound();

        var movie = await _context.Movie
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == id);

        if (movie == null) return NotFound();

        Movie = movie;

        if (saveChangesError.GetValueOrDefault())
            ErrorMessage = "Delete failed. Try again.";

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id == null) return NotFound();

        var movie = await _context.Movie.FindAsync(id);

        if (movie == null) return NotFound();

        try
        {
            _context.Movie.Remove(movie);
            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Delete failed for movie {MovieId}", id);
            return RedirectToPage("./Delete", new { id, saveChangesError = true });
        }
    }
}