// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CarGame.Api.Data;
using CarGame.Api.Entites;
using CarGame.Api.Exceptions;
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

        /// <summary>
        /// Adds a sighting to the database.
        /// </summary>
        /// <param name="sighting">The sighting to add.</param>
        /// <exception cref="AlreadySeenException">Thrown when the sighting has already been recorded.</exception>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task AddSighting(Sighting sighting)
        {
            var alreadySeen = await IsAlreadySeen(sighting).ConfigureAwait(false);
            if (alreadySeen)
            {
                throw new AlreadySeenException();
            }
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

        private async Task<bool> IsAlreadySeen(Sighting sighting)
        {
            return await _context.Sightings
                                 .AnyAsync(s => s.PlateId == sighting.PlateId &&
                                               (!sighting.IsDiplomat || s.DiplomatNumber == sighting.DiplomatNumber))
                                 .ConfigureAwait(false);
        }

    }
}
