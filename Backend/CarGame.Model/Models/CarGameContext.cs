// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.EntityFrameworkCore;

namespace CarGame.Model.Models;

public partial class CarGameContext : DbContext
{
    public CarGameContext()
    {
    }

    public CarGameContext(DbContextOptions<CarGameContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Plate> Plates { get; set; }

    public virtual DbSet<Sighting> Sightings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("postgis");

        modelBuilder.Entity<Plate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("plates_pkey");

            entity.ToTable("plates", "car_game");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Country).HasColumnName("country");
            entity.Property(e => e.CountryAbbreviation).HasColumnName("country_abbreviation");
            entity.Property(e => e.CountryCode)
                .HasMaxLength(2)
                .HasColumnName("country_code");
            entity.Property(e => e.DiplomatCode).HasColumnName("diplomat_code");
            entity.Property(e => e.IsDiplomat).HasColumnName("is_diplomat");
        });

        modelBuilder.Entity<Sighting>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("sightings_pkey");

            entity.ToTable("sightings", "car_game");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Date)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("date");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.DiplomatNumber).HasColumnName("diplomat_number");
            entity.Property(e => e.IsDiplomat).HasColumnName("is_diplomat");
            entity.Property(e => e.Location)
                .HasColumnType("geometry(Point,3067)")
                .HasColumnName("location");
            entity.Property(e => e.PlateId).HasColumnName("plate_id");

            entity.HasOne(d => d.Plate).WithMany(p => p.Sightings)
                .HasForeignKey(d => d.PlateId)
                .HasConstraintName("sightings_plate_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
