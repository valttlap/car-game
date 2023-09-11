// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using AutoMapper;
using CarGame.Api.DTOs;
using CarGame.Api.Exceptions;
using CarGame.Api.Interfaces;
using CarGame.Model.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarGame.Api.Controllers
{
    public class SightingController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICoordinateTransformationService _coordinateTransformationService;

        private readonly IMapper _mapper;
        public SightingController(IUnitOfWork unitOfWork, IMapper mapper, ICoordinateTransformationService coordinateTransformationService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _coordinateTransformationService = coordinateTransformationService;
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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var sighting = _mapper.Map<Sighting>(sightingDto);
            if (sighting.Location.SRID != 3067)
            {
                sighting.Location = _coordinateTransformationService.TransformTo3067(sighting.Location);
            }
            try
            {
                await _unitOfWork.SightingRepository.AddSighting(sighting).ConfigureAwait(false);
            }
            catch (AlreadySeenException)
            {
                return BadRequest("This sighting has already been recorded.");
            }
            catch
            {
                return StatusCode(500, "An error occurred while adding the sighting.");
            }
            await _unitOfWork.Complete().ConfigureAwait(false);
            return Ok(sightingDto);
        }
    }
}
