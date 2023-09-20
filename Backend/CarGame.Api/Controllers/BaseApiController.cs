// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarGame.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class BaseApiController : ControllerBase
{
}
