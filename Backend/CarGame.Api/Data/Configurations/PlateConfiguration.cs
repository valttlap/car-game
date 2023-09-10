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
        builder.Property(p => p.Id).UseIdentityAlwaysColumn();
        builder.Property(p => p.Code).HasMaxLength(2);

        builder.Property(p => p.Country).IsRequired();

        builder.HasData(PlateSeedData.GetPlates());

        builder.ToTable("Plates", t => t.HasCheckConstraint(
            "CK_Plate_IsDiplomat_Code_Abbreviation",
            "((\"is_diplomat\" = TRUE AND \"code\" IS NOT NULL AND \"abbreviation\" IS NULL) OR " +
            "(\"is_diplomat\" = FALSE AND \"abbreviation\" IS NOT NULL AND \"code\" IS NULL))"));
    }
}
