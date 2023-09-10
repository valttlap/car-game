// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CarGame.Api.DTOs;

public class PlateDto
{
    public string Country { get; set; } = default!;
    public string? Abbreviation { get; set; }
    public string? Code { get; set; }
    public bool IsDiplomat { get; set; } = default!;
}
