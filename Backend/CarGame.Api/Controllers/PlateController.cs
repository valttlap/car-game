// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using AutoMapper;
using CarGame.Api.DTOs;
using CarGame.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CarGame.Api.Controllers;

public class PlateController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public PlateController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PlateDto>>> GetPlates()
    {
        var plates = await _unitOfWork.PlateRepository.GetPlatesAsync().ConfigureAwait(false);
        var platesDto = _mapper.Map<IEnumerable<PlateDto>>(plates);
        return Ok(platesDto);
    }

    [HttpGet("diplomat")]
    public async Task<ActionResult<IEnumerable<PlateDto>>> GetDiplomatPlates()
    {
        var plates = await _unitOfWork.PlateRepository.GetDiplomatPlatesAsync().ConfigureAwait(false);
        var platesDto = _mapper.Map<IEnumerable<PlateDto>>(plates);
        return Ok(platesDto);
    }

    [HttpGet("regular")]
    public async Task<ActionResult<IEnumerable<PlateDto>>> GetRegularPlates()
    {
        var plates = await _unitOfWork.PlateRepository.GetRegularPlatesAsync().ConfigureAwait(false);
        var platesDto = _mapper.Map<IEnumerable<PlateDto>>(plates);
        return Ok(platesDto);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PlateDto>> GetPlateById(int id)
    {
        var plate = await _unitOfWork.PlateRepository.GetPlateByIdAsync(id).ConfigureAwait(false);
        var plateDto = _mapper.Map<PlateDto>(plate);
        return Ok(plateDto);
    }

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<PlateDto>>> FindPlatesByAbbr([FromQuery] string abbr,
                                                                            [FromQuery] bool isDiplomat = false)
    {
        var plates = await _unitOfWork.PlateRepository.FindPlatesByAbbrAsync(abbr, isDiplomat).ConfigureAwait(false);
        var platesDto = _mapper.Map<IEnumerable<PlateDto>>(plates);
        return Ok(platesDto);
    }
}
