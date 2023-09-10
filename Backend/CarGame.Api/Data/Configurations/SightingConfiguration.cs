// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CarGame.Api.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarGame.Api.Data.Configurations;

public class SightingConfiguration : IEntityTypeConfiguration<Sighting>
{
    public void Configure(EntityTypeBuilder<Sighting> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).UseIdentityAlwaysColumn();
        builder.Property(s => s.PlateId).IsRequired();
        builder.Property(s => s.Date)
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        builder.Property(s => s.DiplomatNumber)
                .HasPrecision(2);
        builder.Property(s => s.Location).IsRequired().HasColumnType("geometry (point, 3067)");
        builder.HasOne(s => s.Plate)
            .WithMany()
            .HasForeignKey(s => s.PlateId);
        builder.ToTable("Sightings", t => t.HasCheckConstraint("CK_PlateId_DiplomatNumber", @"
        (IsDiplomat = true AND NOT EXISTS (
            SELECT 1 FROM Sightings s2
            WHERE s2.PlateId = PlateId AND s2.DiplomatNumber = DiplomatNumber AND s2.Id != Id
        ))
        OR
        (IsDiplomat = false AND NOT EXISTS (
            SELECT 1 FROM Sightings s2
            WHERE s2.PlateId = PlateId AND s2.Id != Id
        ))"));
        builder.ToTable("Sightings", t => t.HasCheckConstraint("CK_PlateId_DiplomatNumber", "IsDiplomat = false OR (DiplomatNumber IS NOT NULL AND DiplomatNumber >= 1 AND DiplomatNumber <= 99)"));
    }
}
