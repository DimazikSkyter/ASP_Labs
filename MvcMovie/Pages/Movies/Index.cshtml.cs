using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MvcMovie.Data;
using MvcMovie.Models;

namespace MvcMovie.Pages.Movies;

public class IndexModel : PageModel
{
    private readonly MvcMovieContext _context;
    private readonly IConfiguration _configuration;

    public IndexModel(MvcMovieContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public PaginatedList<Movie> Movies { get; set; } = default!;

    public string TitleSort { get; set; } = "";
    public string DateSort { get; set; } = "";
    public string PriceSort { get; set; } = "";

    public string CurrentSort { get; set; } = "";
    public string CurrentFilter { get; set; } = "";

    public async Task OnGetAsync(string? sortOrder, string? currentFilter, string? searchString, int? pageIndex)
    {
        CurrentSort = sortOrder ?? "";

        TitleSort = string.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
        DateSort = sortOrder == "Date" ? "date_desc" : "Date";
        PriceSort = sortOrder == "Price" ? "price_desc" : "Price";

        if (searchString != null)
            pageIndex = 1;
        else
            searchString = currentFilter;

        CurrentFilter = searchString ?? "";

        IQueryable<Movie> moviesIQ = _context.Movie;

        if (!string.IsNullOrWhiteSpace(searchString))
        {
            moviesIQ = moviesIQ.Where(m =>
                m.Title!.Contains(searchString) ||
                m.Genre!.Contains(searchString));
        }

        moviesIQ = sortOrder switch
        {
            "title_desc" => moviesIQ.OrderByDescending(m => m.Title),
            "Date" => moviesIQ.OrderBy(m => m.ReleaseDate),
            "date_desc" => moviesIQ.OrderByDescending(m => m.ReleaseDate),
            "Price" => moviesIQ.OrderBy(m => m.Price),
            "price_desc" => moviesIQ.OrderByDescending(m => m.Price),
            _ => moviesIQ.OrderBy(m => m.Title)
        };

        var pageSize = _configuration.GetValue("PageSize", 4);

        Movies = await PaginatedList<Movie>.CreateAsync(
            moviesIQ.AsNoTracking(),
            pageIndex ?? 1,
            pageSize);
    }
}