﻿namespace ProductsAPI.Models;

// Class to return error details
public class ErrorDetails
{
    public int StatusCode { get; set; }
    public string? Message { get; set; }
    public string? Trace { get; set; }

    public override string? ToString()
    {
        return JsonContent.Create(this).ToString();
    }
}
