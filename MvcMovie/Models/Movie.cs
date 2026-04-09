using System.ComponentModel.DataAnnotations;

namespace MvcMovie.Models;

public class Movie : Production
{
    [Display(Name = "Duration (min)")]
    [Range(1, 500)]
    public int DurationMinutes { get; set; }
}