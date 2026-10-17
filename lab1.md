## Zadanie A1

- Program.cs
Główny plik rozruchowy aplikacji. Zawiera funkcję main - punkt dostępu do aplikacji, zazwyczaj w tym miejscu odbywają inicjalizacje niezbędnych składników aplikacji.
- Controllers
Katalog z kontrolerami, czyli klasami obsługującymi żądania HTTP. Każda publiczna metoda (akcja) przetwarza żądanie i zwraca odpowiedź, najczęściej widok lub dane.
- Models
Katalog z klasami modeli, które reprezentują dane aplikacji i logikę z nimi związaną. Modele przekazuje się między kontrolerami a widokami.
- Views
Katalog z widokami Razor (.cshtml), które generują HTML wysyłany do przeglądarki. Widoki są pogrupowane w podfolderach odpowiadających nazwom kontrolerów, a w folderze Shared znajdują się elementy wspólne, np. layout.
- wwwroot/
Katalog z plikami statycznymi (CSS, JavaScript, obrazy, biblioteki front-endowe), serwowanymi bezpośrednio przeglądarce. Jest to jedyny folder projektu dostępny publicznie przez URL.
- appsettings.json
Plik konfiguracyjny aplikacji w formacie JSON, np. connection stringi, poziomy logowania czy własne ustawienia. Może być nadpisywany przez pliki dla konkretnych środowisk, np. appsettings.Development.json.
- Properties/launchSettings.json
Plik z profilami uruchomieniowymi używanymi lokalnie podczas developmentu (`dotnet run`, debugowanie). Określa m.in. adresy URL i porty, zmienną ASPNETCORE_ENVIRONMENT oraz to, czy otworzyć przeglądarkę. Nie jest używany na produkcji.

## Zadanie B

- Metoda ządania: GET
- Kod statusu: 200
- Content-Type: text/html; charset=utf-8
- Nagłówek User-Agent:
Mozilla/5.0 (Linux; Android 6.0; Nexus 5 Build/MRA58N) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/145.0.0.0 Mobile Safari/537.36
- Ile żądań wykonała strona łącznie i dlaczego więcej niż jedno?
Strona musiała pobrać takze inne niezbędne do jej działania elementy, takie jak
biblioteki CSS, bilbiotekę jQuery oraz inne skrypty JS, pliki z kluczami/wartościami tłumaczeń.
Pobranie kazdego z powyzszych plików wymaga osobnego ządania.
- Przejście na nieistiejącą ściezkę prowadzi do 404

## Zadanie C

- /Book/Status?code=404 - zwraca source, czysty html
- /Book/Status?code=418 - nie zwraca nic, nawet w preview
- /Book/Status?code=500 - zwraca source, czysty html

## Zadanie D
- http://localhost:5169/Book/Echo?name=Ala
Cześć, Ala! - zmiana wartości powoduje zmianę wyświetlanego imienia
- http://localhost:5169/Book/Import
{
    "received": "Solaris",
    "isbn": "9788308065341",
    "price": 39.9
}
- http://localhost:5169/Book/Import (plain text)
Unsupported Media Type (415)
- GET /Book/Import
404

## Zadania do samodzielnego wykonania

1. Dodaj akcję Sum(int a, int b) dostępną pod /Book/Sum?a=2&b=3, zwracającą 5. Co się stanie po ?a=x? Jaki jest kod statusu i dlaczego (model binding)? - 200, model binding nie potrafi przekonwertować tekstu na liczbę, zmienna z query params dostaje domyślny int - 0.
2. Dodaj akcję zwracającą nagłówek User-Agent klienta przez Request.Headers.UserAgent. Wywołaj ją z przeglądarki i z Postmana – porównaj wyniki.
- przeglądarka: Mozilla/5.0 (Linux; Android 6.0; Nexus 5 Build/MRA58N) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/145.0.0.0 Mobile Safari/537.36
- postman: PostmanRuntime/2.11.0
2 rózne środowiska
3. Dodaj akcję Book/Health zwracającą Json(new { status = "ok", time = DateTime.Now }). Zwróć uwagę na nagłówek Content-Type: application/json.
- dodane

## Efekt do zaliczenia
- [x] Działający projekt z BookController i co najmniej 5 akcjami (Hello, Echo, Square, Import, Inspect).
- [x] Wypełniona tabela z części B oraz zrzut ekranu zakładki Network dla /Book/Echo?name=Ala.
- [x] Zrzut ekranu z Postmana pokazujący poprawny POST /Book/Import (200) i błąd 415.

## Pytania kontrolne
1. Czym różnią się metody GET i POST z punktu widzenia idempotentności i miejsca przekazywania danych?
- GET jest bezpieczny i idempotentny: nie zmienia stanu serwera, a wielokrotne wywołanie daje ten sam efekt. Dane przekazuje się w URL (query string), więc są widoczne w historii i logach, mogą trafić do cache i mają ograniczoną długość.
- POST nie jest idempotentny: każde wywołanie może utworzyć nowy zasób albo wywołać skutek uboczny, np. dwa identyczne zamówienia. Dane trafiają w ciało żądania (body), które nie ma praktycznego limitu rozmiaru i nie jest widoczne w URL.
2. Co oznaczają kody 200, 302, 400, 401, 403, 404, 405, 415, 500?
- 200 OK
- 302 Found: przekierowanie tymczasowe pod adres z nagłówka Location.
- 400 Bad Request: niepoprawne żądanie, np. błędne dane lub nieudany model binding.
- 401 Unauthorized: brak uwierzytelnienia; serwer nie wie, kim jesteś.
- 403 Forbidden: serwer wie, kim jesteś, ale nie masz uprawnień.
- 404 Not Found: zasób nie istnieje.
- 405 Method Not Allowed: zasób istnieje, ale nie obsługuje użytej metody, np. GET na akcję z [HttpPost].
- 415 Unsupported Media Type: nieobsługiwany format treści, np. brak Content-Type: application/json przy [FromBody].
- 500 Internal Server Error: nieobsłużony błąd po stronie serwera, np. wyjątek w kodzie.
3. Skąd model binding wie, czy id pochodzi z ścieżki, czy z query string?
- Przeszukuje w ustalonej kolejności: pola formularza, wartości trasy, query string. Używa pierwszej znalezionej wartości.
4. Dlaczego pliki z wwwroot są dostępne pod adresem, a pliki z Controllers/ nie?
- Pliki statyczne serwuje middleware, który mapuje URL-e wyłącznie na katalog wwwroot (tzw. web root). W Program.cs włącza go app.UseStaticFiles(), a w szablonie .NET 9 app.MapStaticAssets(). Middleware nie widzi nic spoza tego katalogu.
Pliki z Controllers/ to kod źródłowy, który jest kompilowany do DLL. Same pliki .cs nie trafiają nawet do publikowanej aplikacji, a URL-e do kontrolerów obsługuje routing, który wywołuje skompilowane akcje.