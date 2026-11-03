using System.ComponentModel.DataAnnotations;
namespace BookStore.Web.Models;

public class BookFormViewModel : IValidatableObject
{
    [Required(ErrorMessage = "Tytuł jest wymagany")]
    [StringLength(200, MinimumLength = 2, ErrorMessage = "Tytuł: 2–200 znaków")]
    [Display(Name = "Tytuł")]
    public string Title { get; set; } = "";

    [Required, Isbn]                          // własny atrybut – część E
    [Display(Name = "ISBN")]
    public string Isbn { get; set; } = "";

    [DataType(DataType.Date)]
    [NotInFuture]   // własny atrybut – część E
    [Display(Name = "Data wydania")]
    public DateTime? PublicationDate { get; set; }

    [Range(1, 20000)]
    [Display(Name = "Liczba stron")]
    public int NumPages { get; set; } = 100;

    [Range(0.01, 10000, ErrorMessage = "Cena musi być dodatnia")]
    [Display(Name = "Cena")]
    public decimal Price { get; set; }

    [Required, Display(Name = "Autor")]
    public string Author { get; set; } = "";

    [RegularExpression(@"^[A-Z]{3}-\d{4}$", ErrorMessage = "Kod wydawcy: 3 wielkie litery, myślnik i 4 cyfry (np. WLT-0001)")]
    [Display(Name = "Kod wydawcy")]
    public string? PublisherCode { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (PublicationDate is DateTime datetime && datetime.Year < 1900 && Price > 100)
        {
            yield return new ValidationResult("Książka sprzed 1900 r. nie może kosztować więcej niż 100 zł");
        }
    }
}