using Microsoft.AspNetCore.Diagnostics;
using ProductsAPI.Models;
using System.Net;

namespace ProductsAPI.Extensions;

public static class ApiExceptionMiddlewareExtensions
{
    public static void ConfigureExceptionHandler(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(appError => // UseExceptionHandler method to handle exceptions globally
        {
            appError.Run(async context => // 
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError; // Set the status code to 500
                context.Response.ContentType = "application/json"; // Set the content type to json

                var contextFeature = context.Features.Get<IExceptionHandlerFeature>(); // Get the exception handler feature
                if (contextFeature is not null)
                {
                    await context.Response.WriteAsync(new ErrorDetails() // Create a new ErrorDetails object
                    {
                        StatusCode = context.Response.StatusCode,
                        Message = contextFeature.Error.Message,
                        Trace = contextFeature.Error.StackTrace
                    }.ToString()); // Write the error details to the response
                }
            });
        });
    }
}

