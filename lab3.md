## Część A

- /books/year/1965 - lista książek z 1965 r. (w danych w pamięci nie ma książki z 1997 r.)
- /books/year/abc - 404, ograniczenie `int` nie pasuje
- /books/year/1200 - 404, `min(1450)`
- /books/isbn/9788308065341 - 302, przekierowanie na /Book/Details/1 (ISBN z własnych danych)
- /books/isbn/123 - 404, regex wymaga 13 cyfr
- /docs/a/b/c - strona z `path = a/b/c`
- /, /Book, /Book/Index - Home/Index, Book/Index, Book/Index

## Zadanie B1

| Adres | Kontroler.Akcja | Metoda HTTP |
|---|---|---|
| /Admin/Authors/Create | Authors.Create | GET |
| /Admin/Authors/Create | Authors.Create | POST |
| /Admin/Authors/Index | Authors.Index | dowolna |
| /Admin/Dashboard/Index | Dashboard.Index | dowolna |
| /Book/Archive | Book.Archive | dowolna |
| /Book/Boom | Book.Boom | dowolna |
| /Book/ByIsbn | Book.ByIsbn | dowolna |
| /Book/ByYear | Book.ByYear | dowolna |
| /Book/Create | Book.Create | GET |
| /Book/Create | Book.Create | POST |
| /Book/Details/{id:int} | Book.Details | GET |
| /Book/Echo | Book.Echo | dowolna |
| /Book/FilterOrder | Book.FilterOrder | dowolna |
| /Book/Hello | Book.Hello | dowolna |
| /Book/Import | Book.Import | POST |
| /Book/Index | Book.Index | dowolna |
| /Book/Inspect | Book.Inspect | dowolna |
| /Book/JsonDemo | Book.JsonDemo | dowolna |
| /Book/Square | Book.Square | dowolna |
| /Book/Status | Book.Status | dowolna |
| /Book/Sum | Book.Sum | dowolna |
| /Book/UserAgent | Book.UserAgent | dowolna |
| /Home/Boom | Home.Boom | dowolna |
| /Home/Docs | Home.Docs | dowolna |
| /Home/Error | Home.Error | dowolna |
| /Home/HttpStatus | Home.HttpStatus | dowolna |
| /Home/Index | Home.Index | dowolna |
| /Home/Privacy | Home.Privacy | dowolna |
| /Home/Routes | Home.Routes | dowolna |

Dodatkowe trasy z `Program.cs`, niewidoczne w `IActionDescriptorCollectionProvider`:

| Adres | Kontroler.Akcja | Metoda HTTP |
|---|---|---|
| /books/year/{year:int:min(1450)} | Book.ByYear | dowolna |
| /books/isbn/{isbn:regex(^\d{13}$)} | Book.ByIsbn | dowolna |
| /docs/{*path} | Home.Docs | dowolna |
| /archive/{year:int}/{month:int:range(1,12)} | Book.Archive | dowolna |

## Część D

- Nagłówki `X-Elapsed-Ms` i `X-Request-Count` są widoczne w zakładce Network.
- Logi w konsoli: `GET /Book -> 200 w 2 ms`
- Które żądania zostaną pominięte po przeniesieniu za `UseStaticFiles()`?
Żądania do plików statycznych (css, js, favicon, lib). `UseStaticFiles` sam zwraca plik i kończy potok, więc żądanie nie dociera do middleware. Takie odpowiedzi nie mają nagłówków i nie są liczone.
- /blokada - 403 z pustą treścią. Middleware nie wywołuje `_next`, więc kontroler ani strona błędu nie są wykonywane (short-circuit).

## Część F

- /nie-ma-takiej-strony - własna strona „Błąd 404”
- /Book/Details/9999 - `NotFound()`, ta sama strona 404
- /Book/Boom - strona 500 z filtra wyjątku, w logu `fail: ... Błąd w BookController.Boom`
- /Book/Status?code=418 - co się zmieniło w porównaniu z Zadaniem C z Lab 1?
Wcześniej odpowiedź 418 była pusta. Teraz `UseStatusCodePagesWithReExecute` wykonuje ponownie `/Home/HttpStatus?code=418`, więc przy tym samym statusie 418 pojawia się strona „Błąd 418 - I'm a teapot”.

## Zadania do samodzielnego wykonania

### 1. Trasa `archive/{year:int}/{month:int:range(1,12)}`

- /archive/1965/3 - lista książek z 1965 r.
- /archive/1965/13 - 404, `range(1,12)`

### 2. Licznik żądań dla każdej ścieżki

`RequestStats` zawiera `ConcurrentDictionary<string,long>` (ścieżka → licznik). /Admin pokazuje 5 najpopularniejszych ścieżek.

### 3. Filtr `[MeasureTime]`

Filtr akcji dodaje nagłówek `X-Action-Ms`, nałożony na `BookController`.

### 4. Kolejność filtrów akcji

/Book/FilterOrder z `[TraceFilter("Pierwszy")]` i `[TraceFilter("Drugi")]`:
```
START BookController.FilterOrder
[Pierwszy] OnActionExecuting
[Drugi] OnActionExecuting
[Drugi] OnActionExecuted
[Pierwszy] OnActionExecuted
KONIEC BookController.FilterOrder
```
- `OnActionExecuting` wykonuje się w kolejności deklaracji, a `OnActionExecuted` w odwrotnej.
- Domyślnie najpierw działają filtry globalne, potem filtry kontrolera (tu `LogActionFilter`), a na końcu filtry akcji.
- Czy jest parametr określający kolejność?
Tak, właściwość `Order` (`IOrderedFilter`, domyślnie 0). Filtr z niższą wartością wykonuje się wcześniej, np. `[TraceFilter("Drugi", Order = -1)]`.

## Efekt do zaliczenia
- [x] Tabela adresów URL z przypisanymi akcjami.
- [x] Działający potok z własnym middleware (nagłówki `X-Elapsed-Ms`, `X-Request-Count`).
- [x] Obszar `Admin` z własnym layoutem, filtr akcji i wyjątku, strony 404/500.
- [x] Wykonane zadania.
