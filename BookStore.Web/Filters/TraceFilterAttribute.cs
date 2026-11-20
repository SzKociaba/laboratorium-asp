using Microsoft.AspNetCore.Mvc.Filters;

namespace BookStore.Web.Filters;

// Zadanie 4: filtr akcji do badania kolejności wykonywania filtrów
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class TraceFilterAttribute : ActionFilterAttribute
{
    private readonly string _name;
    public TraceFilterAttribute(string name) => _name = name;

    public override void OnActionExecuting(ActionExecutingContext c) =>
        Console.WriteLine($"[{_name}] OnActionExecuting");

    public override void OnActionExecuted(ActionExecutedContext c) =>
        Console.WriteLine($"[{_name}] OnActionExecuted");
}
