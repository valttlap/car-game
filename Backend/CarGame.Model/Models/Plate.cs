// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CarGame.Model.Models;

public partial class Plate
{
    public int Id { get; set; }

    public string Country { get; set; } = null!;

    public string? CountryAbbreviation { get; set; }

    public int? DiplomatCode { get; set; }

    public bool IsDiplomat { get; set; }

    public virtual ICollection<Sighting> Sightings { get; set; } = new List<Sighting>();
}
