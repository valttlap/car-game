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
        builder.ToTable("sightings", t => t.HasCheckConstraint("CK_plate_id_diplomat_number", "is_diplomat = false OR (diplomat_number IS NOT NULL AND diplomat_number >= 1 AND diplomat_number <= 99)"));
    }
}
