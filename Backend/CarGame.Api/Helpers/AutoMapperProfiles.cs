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
            .ForMember(dest => dest.Location, opts => opts.MapFrom(src => GeoJsonToGeometry(src.Location, src.SRID)));
        CreateMap<Plate, PlateDto>().ReverseMap();
        CreateMap<Sighting, SightingUserDto>()
            .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
            .ForMember(dest => dest.Location, opts => opts.MapFrom(src => GeometryToGeoJson(src.Location)))
            .ForMember(dest => dest.Country, opts => opts.MapFrom(src => src.Plate.Country))
            .ForMember(dest => dest.SRID, opts => opts.MapFrom(src => src.Location.SRID))
            .ForMember(dest => dest.IsDiplomat, opts => opts.MapFrom(src => src.IsDiplomat))
            .ForMember(dest => dest.DiplomatNumber, opts => opts.MapFrom(src => src.DiplomatNumber));
    }

    private static string? GeometryToGeoJson(Geometry geometry)
    {
        return geometry != null ? new GeoJsonWriter().Write(geometry) : null;
    }

    private static Geometry? GeoJsonToGeometry(string geoJson, int srid = 3067)
    {
        var geom = !string.IsNullOrEmpty(geoJson) ? new GeoJsonReader().Read<Point>(geoJson) : null;
        if (geom != null)
        {
            geom.SRID = srid;
        }
        return geom;
    }
}
