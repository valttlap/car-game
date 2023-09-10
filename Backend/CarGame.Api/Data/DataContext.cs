// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CarGame.Api.Data.Configurations;
using CarGame.Api.Entites;
using Microsoft.EntityFrameworkCore;

namespace CarGame.Api.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }
    public DbSet<Plate> Plates { get; set; } = default!;
    public DbSet<Sighting> Sightings { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new PlateConfiguration());
        modelBuilder.ApplyConfiguration(new SightingConfiguration());
    }
}
