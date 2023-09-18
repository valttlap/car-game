// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations;

namespace CarGame.Api.DTOs;

public class SightingDto
{
    [Required]
    public int PlateId { get; set; }

    public DateTime Date { get; set; }

    public string? Description { get; set; }

    public bool IsDiplomat { get; set; }

    [DiplomatNumberValidation]
    public int? DiplomatNumber { get; set; }

    [Required]
    public string Location { get; set; } = null!;
    public int SRID { get; set; } = 3067;
}

public class DiplomatNumberValidationAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        var sighting = (SightingDto)validationContext.ObjectInstance;

        if (sighting.IsDiplomat)
        {
            if (value == null || value is not int intValue || intValue < 1 || intValue > 99)
            {
                return new ValidationResult("Diplomat number is required and must be between 1 and 99.");
            }
        }
        else
        {
            if (value != null)
            {
                return new ValidationResult("Diplomat number must be null if IsDiplomat is false.");
            }
        }

        return ValidationResult.Success;
    }

}
