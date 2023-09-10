// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CarGame.Api.DTOs;

public class SightingDto
{
    public int PlateId { get; set; }

    public DateTime Date { get; set; }

    public string? Description { get; set; }

    public bool IsDiplomat { get; set; }

    public int? DiplomatNumber { get; set; }

    public string Location { get; set; } = null!;
}
