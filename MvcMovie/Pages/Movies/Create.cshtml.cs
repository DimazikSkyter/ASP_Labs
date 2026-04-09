using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MvcMovie.Data;
using MvcMovie.Models;

namespace MvcMovie.Pages.Movies;

public class CreateModel : PageModel
{
    private readonly MvcMovieContext _context;

    public CreateModel(MvcMovieContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Movie Movie { get; set; } = default!;

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var emptyMovie = new Movie();

        if (await TryUpdateModelAsync(
                emptyMovie,
                "movie",
                m => m.Title,
                m => m.ReleaseDate,
                m => m.Genre,
                m => m.Price,
                m => m.Rating))
        {
            emptyMovie.ConcurrencyToken = Guid.NewGuid();

            _context.Movie.Add(emptyMovie);
            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }

        return Page();
    }
}