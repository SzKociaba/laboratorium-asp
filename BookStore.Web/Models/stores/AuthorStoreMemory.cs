namespace BookStore.Web.Models;

// Lab 3 C1: lista autorów w pamięci (w Lab 5 – baza danych)
public static class AuthorStoreMemory
{
    public static readonly List<Author> Authors = new()
    {
        new() { Id = 1, Name = "Stanisław Lem" },
        new() { Id = 2, Name = "Bolesław Prus" },
    };
}
