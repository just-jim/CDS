using CDS.Application.Assets.Queries.Admin;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CDS.API.Controllers;

[Route("admin/")]
public class AdminController(ISender mediator) : ApiController {
    
    [HttpPost("reset")]
    public async Task<IActionResult> Reset() {
        return await mediator.Send(new ResetQuery()).Match(result => Ok("DB dropped and Cache purged"), Problem);
    }
    
    [HttpPost("drop-db")]
    public async Task<IActionResult> DropDb() {
        return await mediator.Send(new DropDbQuery()).Match(result => Ok("DB dropped"), Problem);
    }
    
    [HttpPost("purge-cache")]
    public async Task<IActionResult> PurgeCache() {
        await mediator.Send(new PurgeCacheQuery());
        return Ok("Cache purged.");
    }
}