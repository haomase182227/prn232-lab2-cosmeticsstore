using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.Reflection;

namespace CosmeticsStore.API.ModelBinders;

/// <summary>
/// Custom model binder to bind query string values to enums using Description attribute
/// Allows swagger to show enum dropdown but API receives kebab-case values
/// </summary>
public class DescriptionEnumBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
        
        if (valueProviderResult == ValueProviderResult.None)
        {
            return Task.CompletedTask;
        }

        var value = valueProviderResult.FirstValue;
        
        if (string.IsNullOrEmpty(value))
        {
            return Task.CompletedTask;
        }

        var enumType = bindingContext.ModelType;
        
        // Handle nullable enums
        if (enumType.IsGenericType && enumType.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
            enumType = Nullable.GetUnderlyingType(enumType)!;
        }

        // Find enum value by Description attribute
        foreach (var field in enumType.GetFields())
        {
            var description = field.GetCustomAttribute<DescriptionAttribute>()?.Description;
            if (description == value || field.Name.Equals(value, StringComparison.OrdinalIgnoreCase))
            {
                var enumValue = Enum.Parse(enumType, field.Name);
                bindingContext.Result = ModelBindingResult.Success(enumValue);
                return Task.CompletedTask;
            }
        }

        // If not found, try default enum parsing
        if (Enum.TryParse(enumType, value, true, out var result))
        {
            bindingContext.Result = ModelBindingResult.Success(result);
            return Task.CompletedTask;
        }

        bindingContext.ModelState.TryAddModelError(
            bindingContext.ModelName,
            $"The value '{value}' is not valid for {bindingContext.ModelName}."
        );

        return Task.CompletedTask;
    }
}
