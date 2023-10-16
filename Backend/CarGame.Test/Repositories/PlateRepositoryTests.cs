// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CarGame.Api.Repositories;
using CarGame.Model.Models;
using Microsoft.EntityFrameworkCore;

namespace CarGame.Test.Repositories;

public class PlateRepositoryTests : IDisposable
{
    private readonly DbContextOptions<CarGameContext> _options;
    private readonly CarGameContext _context;

    public PlateRepositoryTests()
    {
        _options = new DbContextOptionsBuilder<CarGameContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new CarGameContext(_options);

        _context.Plates.AddRange(new List<Plate>
        {
            new() { Id = 1, Country = "Afganistan", CountryAbbreviation = "AFG", IsDiplomat = false, CountryCode = "af" },
            new() { Id = 2, Country = "Alankomaat", CountryAbbreviation = "NL", IsDiplomat = false, CountryCode = "nl" },
            new() { Id = 3, Country = "Saksa", DiplomatCode=10, IsDiplomat = true, CountryCode = "de" },
            new() { Id = 4, Country = "Yhdysvallat", DiplomatCode=12, IsDiplomat = true, CountryCode = "us" }

        });
        _context.SaveChanges();
    }

    public void Dispose()
    {
        _context.Dispose();

        GC.SuppressFinalize(this);
    }

    [Fact]
    public async Task GetAllPlatesAsync_ShouldReturnAllPlates()
    {
        // Arrange
        var repository = new PlateRepository(_context);

        // Act
        var result = await repository.GetPlatesAsync().ConfigureAwait(false);

        // Assert
        Assert.Equal(4, result.Count());
    }

    [Fact]
    public async Task GetRegularPlatesAsync_ShouldReturnAllRegularPlates()
    {
        // Arrange
        var repository = new PlateRepository(_context);

        // Act
        var result = await repository.GetRegularPlatesAsync().ConfigureAwait(false);

        // Assert
        Assert.Equal(2, result.Count());
        Assert.All(result, plate => Assert.False(plate.IsDiplomat));
    }

    [Fact]
    public async Task GetDiplomatPlatesAsync_ShouldReturnAllDiplomatPlates()
    {
        // Arrange
        var repository = new PlateRepository(_context);

        // Act
        var result = await repository.GetDiplomatPlatesAsync().ConfigureAwait(false);

        // Assert
        Assert.Equal(2, result.Count());
        Assert.All(result, plate => Assert.True(plate.IsDiplomat));
    }
}
