// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CarGame.Api.Entites;

namespace CarGame.Api.Interfaces;

public interface ISightingRepository
{
    public Task<IEnumerable<Sighting>> GetSightingsAsync();
    public Task<Sighting?> GetSightingByIdAsync(int id);
    public void AddSighting(Sighting sighting);
}
