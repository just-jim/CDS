using CDS.Contracts.Commands;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CDS.API.Controllers;

[Route("admin/")]
public class AdminController(ISender mediator) : ApiController {

    [HttpDelete("reset")]
    public async Task<IActionResult> Reset() {
        return await mediator.Send(new ResetCommand()).Match(result => Ok("DB dropped and Cache purged"), Problem);
    }

    [HttpDelete("drop-db")]
    public async Task<IActionResult> DropDb() {
        return await mediator.Send(new DropDbCommand()).Match(result => Ok("DB dropped"), Problem);
    }

    [HttpDelete("purge-cache")]
    public async Task<IActionResult> PurgeCache() {
        await mediator.Send(new PurgeCacheCommand());
        return Ok("Cache purged.");
    }
}