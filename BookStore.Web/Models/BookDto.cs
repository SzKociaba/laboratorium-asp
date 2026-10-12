namespace BookStore.Web.Models;

public record BookDto(string Title, string Isbn, decimal Price);

/*
    Mozna uzyć standarowej klasy, ale record daje nam znaczacy skrot.
    Dodatkowo, w record wszystkie pola są generowane z własnością 'init', która
    zapewnia niemutowalność. Record zapewnia tez implementację metod Equals oraz ToString(), co
    bywa bardzo uzyteczne.

    Standardowo utworzona klasa wyglądałaby tak:

    public class BookDto() {
        public BookDto(string title, string isbn, decimal price)
        {
            Title = title; 
            Isbn = isbn; 
            Price = price;
        }

        public string Title { get; init; } // <-- init, brak settera
        public string Isbn { get; init; } // <-- init, brak settera
        public decimal Price { get; init; } // <-- init, brak settera
    }
*/