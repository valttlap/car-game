// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CarGame.Api.Interfaces;

public interface IUnitOfWork
{
    IPlateRepository PlateRepository { get; }
    ISightingRepository SightingRepository { get; }

    /// <summary>
    /// Saves all changes made in this context to the database.
    /// </summary>
    /// <returns>True if the save was successful, false otherwise.</returns>

    /// <summary>
    /// Checks if the context has any changes.
    /// </summary>
    /// <returns>True if the context has changes, false otherwise.</returns>
    Task<bool> Complete();
    bool HasChanges();
}
