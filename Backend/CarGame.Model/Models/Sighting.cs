// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using NetTopologySuite.Geometries;

namespace CarGame.Model.Models;

public partial class Sighting
{
    public int Id { get; set; }

    public int PlateId { get; set; }

    public DateTime Date { get; set; }

    public string? Description { get; set; }

    public bool IsDiplomat { get; set; }

    public int? DiplomatNumber { get; set; }

    public Point Location { get; set; } = null!;

    public virtual Plate Plate { get; set; } = null!;
}
