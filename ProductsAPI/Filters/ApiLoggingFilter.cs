using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace ProductsAPI.Filters;

public class ApiLoggingFilter : IActionFilter
{
    private readonly ILogger<ApiLoggingFilter> _logger;

    public ApiLoggingFilter(ILogger<ApiLoggingFilter> logger)
    {
        _logger = logger;
    }

    // This method is called before the action method is executed.
    public void OnActionExecuting(ActionExecutingContext context)
    {
        _logger.LogInformation($"Action Method Executing - {context.ModelState.IsValid}");
    }

    // This method is called after the action method is executed.
    public void OnActionExecuted(ActionExecutedContext context)
    {
        _logger.LogInformation($"Action Method Executed - {context.ModelState.IsValid}");
    }
}
