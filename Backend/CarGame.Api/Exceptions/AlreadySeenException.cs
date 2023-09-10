// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CarGame.Api.Exceptions;

public class AlreadySeenException : Exception
{
    public AlreadySeenException() : base() { }

    public AlreadySeenException(string message) : base(message) { }

    public AlreadySeenException(string message, Exception innerException) : base(message, innerException) { }
}
