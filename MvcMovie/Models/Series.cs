using System.ComponentModel.DataAnnotations;

namespace MvcMovie.Models;

public class Series : Production
{
    [Display(Name = "Seasons")]
    [Range(1, 100)]
    public int SeasonsCount { get; set; }
}