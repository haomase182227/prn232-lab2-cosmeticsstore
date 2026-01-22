using CosmeticsStore.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CosmeticsStore.API.Filters;

/// <summary>
/// Validation Filter to handle model validation errors
/// Following PRN232 requirement: proper validation with clear error messages
/// </summary>
public class ValidationFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var errors = context.ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .SelectMany(x => x.Value!.Errors.Select(e => 
                    string.IsNullOrEmpty(e.ErrorMessage) 
                        ? e.Exception?.Message ?? "Validation error" 
                        : e.ErrorMessage))
                .ToList();

            var response = ApiResponse.ErrorResponse(
                "Validation failed",
                errors
            );

            context.Result = new BadRequestObjectResult(response);
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        // No action needed after execution
    }
}
