using Microsoft.EntityFrameworkCore;
using MvcMovie.Models;

namespace MvcMovie.Data;

public class MvcMovieContext : DbContext
{
    public MvcMovieContext(DbContextOptions<MvcMovieContext> options)
        : base(options)
    {
    }

    public DbSet<Production> Productions { get; set; } = default!;
    public DbSet<Movie> Movies { get; set; } = default!;
    public DbSet<Series> Series { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Production>()
            .HasDiscriminator<string>("ProductionType")
            .HasValue<Movie>("Movie")
            .HasValue<Series>("Series");
    }
}