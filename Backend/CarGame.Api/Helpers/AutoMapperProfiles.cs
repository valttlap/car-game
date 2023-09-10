// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using AutoMapper;
using CarGame.Api.DTOs;
using CarGame.Model.Models;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;

namespace CarGame.Api.Helpers;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<Sighting, SightingDto>()
            .ForMember(dest => dest.Location, opts => opts.MapFrom(src => GeometryToGeoJson(src.Location)))
            .ReverseMap()
            .ForMember(dest => dest.Location, opts => opts.MapFrom(src => GeoJsonToGeometry(src.Location)));
        CreateMap<Plate, PlateDto>().ReverseMap();
    }

    private static string? GeometryToGeoJson(Geometry geometry)
    {
        return geometry != null ? new GeoJsonWriter().Write(geometry) : null;
    }

    private static Geometry? GeoJsonToGeometry(string geoJson)
    {
        return !string.IsNullOrEmpty(geoJson) ? new GeoJsonReader().Read<Geometry>(geoJson) : null;
    }
}