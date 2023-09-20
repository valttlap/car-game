// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using AutoMapper;
using CarGame.Api.DTOs;
using CarGame.Api.Exceptions;
using CarGame.Api.Interfaces;
using CarGame.Model.Models;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

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
        [OpenApiOperation("GetSightings", "Retrieves all sightings.")]
        [ProducesResponseType(typeof(IEnumerable<SightingUserDto>), 200)]
        public async Task<ActionResult<IEnumerable<SightingDto>>> GetSightings()
        {
            var sightings = await _unitOfWork.SightingRepository.GetSightingsAsync().ConfigureAwait(false);
            return Ok(sightings);
        }

        [HttpGet("{id}")]
        [OpenApiOperation("GetSightingById", "Retrieves a sighting by its ID.")]
        [ProducesResponseType(typeof(SightingDto), 200)]
        [ProducesResponseType(typeof(void), 404)]
        public async Task<ActionResult<SightingDto>> GetSightingById(int id)
        {
            var sighting = await _unitOfWork.SightingRepository.GetSightingByIdAsync(id).ConfigureAwait(false);
            if (sighting == null)
            {
                return NotFound();
            }
            var sightingDto = _mapper.Map<SightingDto>(sighting);
            return Ok(sightingDto);
        }

        [HttpPost]
        [OpenApiOperation("AddSighting", "Adds a new sighting to the database.")]
        [ProducesResponseType(typeof(SightingDto), 200)]
        [ProducesResponseType(typeof(void), 400)]
        [ProducesResponseType(typeof(void), 500)]
        public async Task<ActionResult<SightingDto>> AddSighting(SightingDto sightingDto)
        {
            if (!ModelState.IsValid || !await _unitOfWork.PlateRepository.PlateExistsAsync(sightingDto.PlateId).ConfigureAwait(false))
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
