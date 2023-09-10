// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CarGame.Api.DTOs;

public class PlateDto
{
    public int Id { get; set; }

    public string Country { get; set; } = null!;

    public string? CountryAbbreviation { get; set; }

    public int? DiplomatCode { get; set; }

    public bool IsDiplomat { get; set; }
}
