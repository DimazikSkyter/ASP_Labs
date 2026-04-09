using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MvcMovie.Data;
using MvcMovie.Models;

namespace MvcMovie.Pages.Movies;

public class DetailsModel : PageModel
{
    private readonly MvcMovieContext _context;

    public DetailsModel(MvcMovieContext context)
    {
        _context = context;
    }

    public Movie Movie { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null) return NotFound();

        var movie = await _context.Movie
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == id);

        if (movie == null) return NotFound();

        Movie = movie;
        return Page();
    }
}