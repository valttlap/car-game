// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CarGame.Api.Data.Seed;
using CarGame.Api.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarGame.Api.Data.Configurations;

public class PlateConfiguration : IEntityTypeConfiguration<Plate>
{
    public void Configure(EntityTypeBuilder<Plate> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).UseIdentityAlwaysColumn();

        var plates = PlateSeedData.GetPlates();
        builder.HasData(plates);

        builder.ToTable("Plates", t => t.HasCheckConstraint(
            "CK_Plate_IsDiplomat_Code_Abbreviation",
            "((\"IsDiplomat\" = TRUE AND \"Code\" IS NOT NULL AND \"Abbreviation\" IS NULL) OR " +
            "(\"IsDiplomat\" = FALSE AND \"Abbreviation\" IS NOT NULL AND \"Code\" IS NULL))"));
    }
}
