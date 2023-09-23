// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CarGame.Api.DTOs;
using CarGame.Model.Models;

namespace CarGame.Api.Interfaces;

public interface ISightingRepository
{
    public Task<IEnumerable<SightingUserDto>> GetSightingsAsync();
    public Task<Sighting?> GetSightingByIdAsync(int id);
    public Task AddSighting(Sighting sighting);
    public Task DeleteSighting(int id);
}
