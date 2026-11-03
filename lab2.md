## Zadanie B1

Widok `Views/Book/Index.cshtml` definiuje sekcję `Styles` z regułą CSS kolorującą nagłówek tabeli. Layout renderuje ją w `<head>` przez `@await RenderSectionAsync("Styles", required: false)`, więc w zakładce Elements blok `<style>` widać tuż przed `</head>`, po arkuszach Bootstrapa i `site.css`.

- Co się stanie, gdy widok zdefiniuje sekcję, której layout nie renderuje?
  Strona się nie wyrenderuje. Razor zgłasza wyjątek `InvalidOperationException: The following sections have been defined but have not been rendered by the page at '/Views/Shared/_Layout.cshtml': 'NazwaSekcji'`. Każda sekcja zdefiniowana w widoku musi zostać wyrenderowana przez layout (`RenderSection`/`RenderSectionAsync`). Można to obejść w layoucie wywołaniem `IgnoreSection("NazwaSekcji")`.
  Odwrotna sytuacja: gdy layout renderuje sekcję z `required: true`, a widok jej nie definiuje, też poleci wyjątek. Przy `required: false` brak sekcji jest w porządku.

## Zadania do samodzielnego wykonania

### 1. Kod wydawcy z `[RegularExpression]`

### 2. `IValidatableObject` – „książka sprzed 1900 r. nie może kosztować więcej niż 100 zł”

- Gdzie wyświetlić komunikat?
  `ValidationResult` zwrócony bez nazw pól trafia do `ModelState` pod pustym kluczem, czyli jako błąd na poziomie modelu. Wyświetla go `<div asp-validation-summary="ModelOnly">` na górze formularza (tryb `ModelOnly` pokazuje tylko takie błędy, bez błędów poszczególnych pól). Gdyby podać nazwę pola, np. `new ValidationResult("…", new[] { nameof(Price) })`, komunikat pokazałby się pod polem Cena, w `asp-validation-for="Price"`.
- Uwagi: `Validate` jest wywoływane dopiero, gdy wszystkie atrybuty przejdą walidację, i działa tylko po stronie serwera (brak walidacji klienckiej).

### 3. Sekcja `Scripts` w `Index` i sekcja `Footer` w layoucie
