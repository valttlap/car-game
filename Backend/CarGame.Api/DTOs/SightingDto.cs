// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using NetTopologySuite.Geometries;

namespace CarGame.Api.DTOs;

public class SightingDto
{
    public int PlateId { get; set; }
    public string? Description { get; set; }
    public bool IsDiplomat { get; set; }
    public int DiplomatNumber { get; set; }
    public Point Location { get; set; } = default!;
}
