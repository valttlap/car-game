// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using NetTopologySuite.Geometries;

namespace CarGame.Api.Interfaces;

public interface ICoordinateTransformationService
{
    Point TransformTo3067(Point point);
}
