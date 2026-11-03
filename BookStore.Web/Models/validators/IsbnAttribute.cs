using System.ComponentModel.DataAnnotations;
namespace BookStore.Web.Models;

public class IsbnAttribute : ValidationAttribute
{
    public bool RequireChecksum { get; set; } = false;

    public IsbnAttribute() => ErrorMessage = "Niepoprawny numer ISBN";

    public override bool IsValid(object? value)
    {
        if (value is null)
        {
            ErrorMessage = "Isbn nie moze mieć wartości null";
            return false;
        }

        if (value is not string isbn)
        {
            ErrorMessage = "Isbn musi być podany jako łańcuch";
            return false;
        }

        var digits = isbn.Replace("-", "").Replace(" ", "");

        if (!digits.All(char.IsDigit))
        {
            ErrorMessage = "Isbn musi składać się jedynie z cyfr";
            return false;
        }

        if (digits.Length != 10 && digits.Length != 13)
            return false;

        if (!RequireChecksum)
            return true;

        if (digits.Length == 13)
            return IsValidIsbn13Checksum(digits);

        return true;
    }

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