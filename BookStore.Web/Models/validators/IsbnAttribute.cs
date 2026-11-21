using System.ComponentModel.DataAnnotations;
namespace BookStore.Web.Models;

public class IsbnAttribute : ValidationAttribute
{
    public bool RequireChecksum { get; set; } = false;

    public IsbnAttribute() => ErrorMessage = "Niepoprawny numer ISBN";

    // Instancja atrybutu jest współdzielona przez wszystkie żądania, więc nie zmieniamy
    // ErrorMessage – komunikat dla konkretnego błędu zwracamy w ValidationResult.
    protected override ValidationResult? IsValid(object? value, ValidationContext context)
    {
        if (value is null)
            return Error(context, "Isbn nie moze mieć wartości null");

        if (value is not string isbn)
            return Error(context, "Isbn musi być podany jako łańcuch");

        var digits = isbn.Replace("-", "").Replace(" ", "");

        if (!digits.All(char.IsDigit))
            return Error(context, "Isbn musi składać się jedynie z cyfr");

        if (digits.Length != 10 && digits.Length != 13)
            return Error(context, "Isbn musi mieć 10 lub 13 cyfr");

        if (RequireChecksum && digits.Length == 13 && !IsValidIsbn13Checksum(digits))
            return Error(context, ErrorMessage!);   // domyślny: „Niepoprawny numer ISBN”

        return ValidationResult.Success;
    }

    private static ValidationResult Error(ValidationContext context, string message) =>
        new(message, context.MemberName is { } member ? new[] { member } : null);

    private static bool IsValidIsbn13Checksum(string digits)
    {
        int sum = 0;

        for (int i = 0; i < 13; i++)
        {
            int d = digits[i] - '0';
            int weight = (i % 2 == 0) ? 1 : 3;
            sum += d * weight;
        }

        return sum % 10 == 0;
    }
}