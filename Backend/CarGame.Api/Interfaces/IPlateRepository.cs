// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CarGame.Api.Entites;

namespace CarGame.Api.Interfaces;

public interface IPlateRepository
{
    public Task<IEnumerable<Plate>> GetPlatesAsync();
    public Task<IEnumerable<Plate>> GetRegularPlatesAsync();
    public Task<IEnumerable<Plate>> GetDiplomatPlatesAsync();
    public Task<Plate?> GetPlateByIdAsync(int id);
    public Task<IEnumerable<Plate>> FindPlatesByAbbrAsync(string abbr, bool isDiplomat);
}
