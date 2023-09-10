// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using AutoMapper;
using CarGame.Api.DTOs;
using CarGame.Api.Entites;
using CarGame.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CarGame.Api.Controllers
{
    public class SightingController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public SightingController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SightingDto>>> GetSightings()
        {
            var sightings = await _unitOfWork.SightingRepository.GetSightingsAsync().ConfigureAwait(false);
            var sightingsDto = _mapper.Map<IEnumerable<SightingDto>>(sightings);
            return Ok(sightingsDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SightingDto>> GetSightingById(int id)
        {
            var sighting = await _unitOfWork.SightingRepository.GetSightingByIdAsync(id).ConfigureAwait(false);
            var sightingDto = _mapper.Map<SightingDto>(sighting);
            return Ok(sightingDto);
        }

        [HttpPost]
        public async Task<ActionResult<SightingDto>> AddSighting(SightingDto sightingDto)
        {
            var sighting = _mapper.Map<Sighting>(sightingDto);
            _unitOfWork.SightingRepository.AddSighting(sighting);
            await _unitOfWork.Complete().ConfigureAwait(false);
            return Ok(sightingDto);
        }
    }
}
