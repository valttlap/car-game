// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CarGame.Api.Data;
using CarGame.Api.Entites;
using CarGame.Api.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CarGame.Api.Repositories;

public class PlateRepository : IPlateRepository
{
    private readonly DataContext _context;

    public PlateRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Plate>> FindPlatesByAbbrAsync(string abbr, bool isDiplomat)
    {
        return await _context.Plates
                             .Where(p => p.IsDiplomat == !isDiplomat &&
                                         (isDiplomat ? p.Code : p.Abbreviation)!.StartsWith(abbr))
                             .ToListAsync()
                             .ConfigureAwait(false);
    }

    public async Task<IEnumerable<Plate>> GetDiplomatPlatesAsync()
    {
        return await _context.Plates.Where(p => p.IsDiplomat).ToListAsync().ConfigureAwait(false);
    }

    public async Task<Plate?> GetPlateByIdAsync(int id)
    {
        return await _context.Plates.FindAsync(id).ConfigureAwait(false);
    }

    public async Task<IEnumerable<Plate>> GetPlatesAsync()
    {
        return await _context.Plates.ToListAsync().ConfigureAwait(false);
    }

    public async Task<IEnumerable<Plate>> GetRegularPlatesAsync()
    {
        return await _context.Plates.Where(p => !p.IsDiplomat).ToListAsync().ConfigureAwait(false);
    }
}
