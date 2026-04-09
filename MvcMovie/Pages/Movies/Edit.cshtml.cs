using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MvcMovie.Data;
using MvcMovie.Models;

namespace MvcMovie.Pages.Movies;

public class EditModel : PageModel
{
    private readonly MvcMovieContext _context;

    public EditModel(MvcMovieContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Movie Movie { get; set; } = default!;

    public string? ConcurrencyErrorMessage { get; set; }

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

    public async Task<IActionResult> OnPostAsync(int id)
    {
        if (id != Movie.Id) return NotFound();

        var movieToUpdate = await _context.Movie.FirstOrDefaultAsync(m => m.Id == id);
        if (movieToUpdate == null)
        {
            ModelState.AddModelError(string.Empty, "Запись уже была удалена другим пользователем.");
            return Page();
        }

        var oldToken = Movie.ConcurrencyToken;

        if (await TryUpdateModelAsync(
                movieToUpdate,
                "movie",
                m => m.Title,
                m => m.ReleaseDate,
                m => m.Genre,
                m => m.Price,
                m => m.Rating))
        {
            movieToUpdate.ConcurrencyToken = Guid.NewGuid();

            _context.Entry(movieToUpdate)
                .Property(m => m.ConcurrencyToken)
                .OriginalValue = oldToken;

            try
            {
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }
            catch (DbUpdateConcurrencyException)
            {
                var databaseValues = await _context.Entry(movieToUpdate).GetDatabaseValuesAsync();

                if (databaseValues == null)
                {
                    ModelState.AddModelError(string.Empty,
                        "Сохранение невозможно. Запись удалена другим пользователем.");
                }
                else
                {
                    var dbMovie = (Movie)databaseValues.ToObject();

                    ModelState.AddModelError(string.Empty,
                        "Запись была изменена другим пользователем. Текущие значения из БД показаны ниже. " +
                        "Если хочешь всё равно сохранить свои изменения, нажми Save ещё раз.");

                    ModelState.AddModelError("Movie.Title", $"Текущее значение: {dbMovie.Title}");
                    ModelState.AddModelError("Movie.ReleaseDate", $"Текущее значение: {dbMovie.ReleaseDate:yyyy-MM-dd}");
                    ModelState.AddModelError("Movie.Genre", $"Текущее значение: {dbMovie.Genre}");
                    ModelState.AddModelError("Movie.Price", $"Текущее значение: {dbMovie.Price}");
                    ModelState.AddModelError("Movie.Rating", $"Текущее значение: {dbMovie.Rating}");

                    Movie.ConcurrencyToken = dbMovie.ConcurrencyToken;
                }
            }
        }

        return Page();
    }
}