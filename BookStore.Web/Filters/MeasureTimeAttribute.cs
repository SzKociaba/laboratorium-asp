using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BookStore.Web.Filters;

// Zadanie 3: filtr akcji dodający nagłówek X-Action-Ms
// Stoper trzymamy w HttpContext.Items – instancja atrybutu jest współdzielona przez wszystkie żądania.
public class MeasureTimeAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext c) =>
        c.HttpContext.Items["sw"] = Stopwatch.StartNew();

    public override void OnActionExecuted(ActionExecutedContext c)
    {
        if (c.HttpContext.Items["sw"] is Stopwatch sw)
            c.HttpContext.Response.Headers["X-Action-Ms"] = sw.ElapsedMilliseconds.ToString();
    }
}
