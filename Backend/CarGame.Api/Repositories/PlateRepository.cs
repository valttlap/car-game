// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Globalization;
using CarGame.Api.Interfaces;
using CarGame.Model.Models;
using Microsoft.EntityFrameworkCore;

namespace CarGame.Api.Repositories;

public class PlateRepository : IPlateRepository
{
    private readonly CarGameContext _context;

    public PlateRepository(CarGameContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Plate>> FindPlatesByAbbrAsync(string? abbr, bool? isDiplomat = false)
    {
        if (string.IsNullOrEmpty(abbr))
        {
            if (isDiplomat.HasValue && isDiplomat.Value)
            {
                return await GetDiplomatPlatesAsync().ConfigureAwait(false);
            }
            else
            {
                return await GetRegularPlatesAsync().ConfigureAwait(false);
            }
        }
        abbr = abbr.ToUpper();

        IQueryable<Plate> query = _context.Plates;

        if (isDiplomat.HasValue && isDiplomat.Value)
        {
            query = query.Where(p => p.IsDiplomat && p.DiplomatCode.HasValue);
        }
        else
        {
            query = query.Where(p => !p.IsDiplomat && p.CountryAbbreviation != null && p.CountryAbbreviation.StartsWith(abbr));
        }

        var plates = await query.ToListAsync().ConfigureAwait(false);

        if (isDiplomat.HasValue && isDiplomat.Value)
        {
            plates = plates.Where(p => p.DiplomatCode.HasValue && p.DiplomatCode.Value.ToString(CultureInfo.InvariantCulture).StartsWith(abbr)).ToList();
        }

        return plates;
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

    public async Task<bool> PlateExistsAsync(int id)
    {
        return await _context.Plates.AnyAsync(p => p.Id == id).ConfigureAwait(false);
    }

}
