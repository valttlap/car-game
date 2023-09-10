// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CarGame.Api.Data;
using CarGame.Api.Entites;
using CarGame.Api.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CarGame.Api.Repositories
{
    public class SightingRepository : ISightingRepository
    {
        private readonly DataContext _context;

        public SightingRepository(DataContext context)
        {
            _context = context;
        }

        public void AddSighting(Sighting sighting)
        {
            _context.Sightings.Add(sighting);
        }

        public async Task<Sighting?> GetSightingByIdAsync(int id)
        {
            return await _context.Sightings.FindAsync(id).ConfigureAwait(false);
        }

        public async Task<IEnumerable<Sighting>> GetSightingsAsync()
        {
            return await _context.Sightings.ToListAsync().ConfigureAwait(false);
        }
    }
}
