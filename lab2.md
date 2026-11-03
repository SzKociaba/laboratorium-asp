## Zadanie B1

Widok `Views/Book/Index.cshtml` definiuje sekcję `Styles` z regułą CSS kolorującą nagłówek tabeli. Layout renderuje ją w `<head>` przez `@await RenderSectionAsync("Styles", required: false)`, więc w zakładce Elements blok `<style>` widać tuż przed `</head>`, po arkuszach Bootstrapa i `site.css`.

- Co się stanie, gdy widok zdefiniuje sekcję, której layout nie renderuje?
Strona się nie wyrenderuje. Razor zgłasza wyjątek `InvalidOperationException: The following sections have been defined but have not been rendered by the page at '/Views/Shared/_Layout.cshtml': 'NazwaSekcji'`. Każda sekcja zdefiniowana w widoku musi zostać wyrenderowana przez layout (`RenderSection`/`RenderSectionAsync`). Można to obejść w layoucie wywołaniem `IgnoreSection("NazwaSekcji")`.
Odwrotna sytuacja: gdy layout renderuje sekcję z `required: true`, a widok jej nie definiuje, też poleci wyjątek. Przy `required: false` brak sekcji jest w porządku.
