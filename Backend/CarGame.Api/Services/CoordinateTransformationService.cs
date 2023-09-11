// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text;
using CarGame.Api.Interfaces;
using GeoAPI.CoordinateSystems.Transformations;
using NetTopologySuite.Geometries;
using ProjNet.Converters.WellKnownText;
using ProjNet.CoordinateSystems;
using ProjNet.CoordinateSystems.Transformations;

namespace CarGame.Api.Services;

public class CoordinateTransformationService : ICoordinateTransformationService
{
    private readonly IMathTransform _transformTo3067;

    public CoordinateTransformationService()
    {
        var wgs84 = GeographicCoordinateSystem.WGS84;
        var wkt = "PROJCS[\"ETRS89 / TM35FIN(E,N)\",GEOGCS[\"ETRS89\",DATUM[\"European_Terrestrial_Reference_System_1989\",SPHEROID[\"GRS 1980\",6378137,298.257222101,AUTHORITY[\"EPSG\",\"7019\"]],TOWGS84[0,0,0,0,0,0,0],AUTHORITY[\"EPSG\",\"6258\"]],PRIMEM[\"Greenwich\",0,AUTHORITY[\"EPSG\",\"8901\"]],UNIT[\"degree\",0.0174532925199433,AUTHORITY[\"EPSG\",\"9122\"]],AUTHORITY[\"EPSG\",\"4258\"]],PROJECTION[\"Transverse_Mercator\"],PARAMETER[\"latitude_of_origin\",0],PARAMETER[\"central_meridian\",27],PARAMETER[\"scale_factor\",0.9996],PARAMETER[\"false_easting\",500000],PARAMETER[\"false_northing\",0],UNIT[\"metre\",1,AUTHORITY[\"EPSG\",\"9001\"]],AXIS[\"Easting\",EAST],AXIS[\"Northing\",NORTH],AUTHORITY[\"EPSG\",\"3067\"]]";

        var cf = new CoordinateSystemFactory();
        var f = new CoordinateTransformationFactory();

        var sys3067 = cf.CreateFromWkt(wkt);

        _transformTo3067 = f.CreateFromCoordinateSystems(wgs84, sys3067).MathTransform;
    }

    public Point TransformTo3067(Point point)
    {
        if (point == null)
        {
            throw new ArgumentNullException(nameof(point));
        }

        var fromPoint = new double[] { point.X, point.Y };
        var toPoint = _transformTo3067.Transform(fromPoint);

        return new Point(toPoint[0], toPoint[1])
        {
            SRID = 3067
        };
    }
}
