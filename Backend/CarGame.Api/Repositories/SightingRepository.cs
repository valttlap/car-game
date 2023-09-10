// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using AutoMapper;
using CarGame.Api.Data;
using CarGame.Api.Interfaces;

namespace CarGame.Api.Repositories
{
    public class SightingRepository : ISightingRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public SightingRepository(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }
    }
}
