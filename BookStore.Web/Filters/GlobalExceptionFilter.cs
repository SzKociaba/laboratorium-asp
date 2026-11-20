using System.Diagnostics;
using BookStore.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace BookStore.Web.Filters;

// Filtr wyjątku: loguje błąd i zwraca widok Error ze statusem 500 (dla /api – ProblemDetails)
public class GlobalExceptionFilter : IExceptionFilter
{
    private readonly ILogger<GlobalExceptionFilter> _log;
    private readonly IModelMetadataProvider _metadata;

    public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> log, IModelMetadataProvider metadata)
    {
        _log = log;
        _metadata = metadata;
    }

    public void OnException(ExceptionContext c)
    {
        _log.LogError(c.Exception, "Błąd w {Action}", c.ActionDescriptor.DisplayName);

        if (c.HttpContext.Request.Path.StartsWithSegments("/api"))
        {
            c.Result = new ObjectResult(new ProblemDetails
            {
                Status = 500,
                Title = "Wystąpił błąd serwera",
                Detail = c.Exception.Message
            }) { StatusCode = 500 };
        }
        else
        {
            c.Result = new ViewResult
            {
                ViewName = "Error",
                StatusCode = 500,
                ViewData = new ViewDataDictionary<ErrorViewModel>(_metadata, c.ModelState)
                {
                    Model = new ErrorViewModel { RequestId = Activity.Current?.Id ?? c.HttpContext.TraceIdentifier }
                }
            };
        }

        c.ExceptionHandled = true;
    }
}
