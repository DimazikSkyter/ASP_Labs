using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcMovie.Models;

public class Movie
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Без наименования нельзя!")]
    public string? Title { get; set; }

    [Display(Name = "Release Date")]
    [DataType(DataType.Date)]
    public DateTime ReleaseDate { get; set; }

    [RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$"), Required, StringLength(30)]
    public string? Genre { get; set; }

    
    [Column(TypeName = "decimal(18, 2)")]
    public decimal Price { get; set; }
    
    public string? Rating { get; set; }
}