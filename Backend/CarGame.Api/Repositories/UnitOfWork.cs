// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using AutoMapper;
using CarGame.Api.Data;
using CarGame.Api.Interfaces;

namespace CarGame.Api.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public UnitOfWork(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public IPlateRepository PlateRepository => new PlateRepository(_context);

    public ISightingRepository SightingRepository => new SightingRepository(_context);

    public async Task<bool> Complete()
    {
        return await _context.SaveChangesAsync().ConfigureAwait(false) > 0;
    }

    public bool HasChanges()
    {
        return _context.ChangeTracker.HasChanges();
    }
}
