using BookStore.Web.Filters;
using BookStore.Web.Middleware;

namespace BookStore.Web;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        // Lab 3 E: filtr wyjątku zarejestrowany globalnie (dla wszystkich kontrolerów)
        builder.Services.AddControllersWithViews(o => o.Filters.Add<GlobalExceptionFilter>());

        // Lab 3 D: licznik żądań – jedna instancja na całą aplikację
        builder.Services.AddSingleton<RequestStats>();
        // Lab 3 E: filtr akcji pobierany z DI przez [ServiceFilter(typeof(LogActionFilter))]
        builder.Services.AddScoped<LogActionFilter>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        // Lab 3 D: własne middleware na początku potoku – mierzy wszystkie żądania
        app.UseMiddleware<RequestTimingMiddleware>();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        // Lab 3 F: własne strony dla odpowiedzi 4xx/5xx bez treści (np. 404)
        app.UseStatusCodePagesWithReExecute("/Home/HttpStatus", "?code={0}");

        app.UseHttpsRedirection();
        app.UseRouting();

        app.UseAuthorization();

        app.MapStaticAssets();

        // Lab 3 A: trasy konwencjonalne – od najbardziej szczegółowej do najbardziej ogólnej
        app.MapControllerRoute("books-by-year", "books/year/{year:int:min(1450)}",
            new { controller = "Book", action = "ByYear" });                    // ograniczenie int + min
        app.MapControllerRoute("books-by-isbn", "books/isbn/{isbn:regex(^\\d{{13}}$)}",
            new { controller = "Book", action = "ByIsbn" });                    // regex: dokładnie 13 cyfr
        app.MapControllerRoute("docs", "docs/{*path}",
            new { controller = "Home", action = "Docs" });                      // catch-all

        // Lab 3 – zadanie 1: miesiąc tylko ilustruje ograniczenie range(1,12)
        app.MapControllerRoute("archive", "archive/{year:int}/{month:int:range(1,12)}",
            new { controller = "Book", action = "Archive" });

        // Lab 3 C: trasa obszarów – przed trasą default
        app.MapControllerRoute("admin-area", "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}")                  // segmenty opcjonalne
            .WithStaticAssets();

        app.Run();
    }
}
