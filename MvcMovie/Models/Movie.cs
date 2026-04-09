using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcMovie.Models;

public class Movie
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Без наименования нельзя!")]
    [StringLength(100)]
    public string? Title { get; set; }

    [Display(Name = "Release Date")]
    [DataType(DataType.Date)]
    public DateTime ReleaseDate { get; set; }

    [RegularExpression(@"^[A-ZА-ЯЁ]+[a-zA-Zа-яА-ЯёЁ\s]*$")]
    [Required]
    [StringLength(30)]
    public string? Genre { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    [Range(0.01, 100000)]
    public decimal Price { get; set; }

    [StringLength(10)] public string? Rating { get; set; }

    [ConcurrencyCheck] public Guid ConcurrencyToken { get; set; } = Guid.NewGuid();
}