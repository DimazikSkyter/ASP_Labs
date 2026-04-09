using Microsoft.EntityFrameworkCore;
using MvcMovie.Data;

namespace MvcMovie.Models;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using var context = new MvcMovieContext(
            serviceProvider.GetRequiredService<DbContextOptions<MvcMovieContext>>());

        if (context.Productions.Any())
        {
            return;
        }
        Console.WriteLine("SeedData.Initialize called");
        context.Productions.AddRange(
            new Movie
            {
                Title = "When Harry Met Sally",
                ReleaseDate = DateTime.Parse("1989-2-12"),
                Genre = "Romantic Comedy",
                Price = 7.99M,
                Rating = "A",
                DurationMinutes = 96
            },
            new Movie
            {
                Title = "Ghostbusters",
                ReleaseDate = DateTime.Parse("1984-3-13"),
                Genre = "Comedy",
                Price = 8.99M,
                Rating = "B",
                DurationMinutes = 105
            },
            new Series
            {
                Title = "Friends",
                ReleaseDate = DateTime.Parse("1994-9-22"),
                Genre = "Comedy",
                Price = 19.99M,
                Rating = "A",
                SeasonsCount = 10
            }
        );
        Console.WriteLine("Saving seed data...");
        context.SaveChanges();
    }
}