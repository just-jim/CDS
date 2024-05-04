using CDS.Application.Assets.Queries.GetAsset;
using CDS.Application.Assets.Queries.GetAssetMetadata;
using CDS.Application.Assets.Queries.ListAssets;
using CDS.Contracts;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CDS.API.Controllers;

[Route("assets/")]
public class AssetsController(ISender mediator) : ApiController {

    [HttpGet]
    public async Task<IActionResult> ListAssets([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10) {
        var query = new ListAssetsQuery { PageNumber = pageNumber, PageSize = pageSize };
        ErrorOr<List<AssetShortResponse>> listAssetsResponse = await mediator.Send(query);

        return listAssetsResponse.Match(
            Ok,
            Problem
        );
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsset(string id) {
        ErrorOr<string> assetFileUrl = await mediator.Send(new GetAssetQuery(id));

        return assetFileUrl.Match(
            Redirect,
            Problem
        );
    }

    [HttpGet("{id}/metadata")]
    public async Task<IActionResult> GetAssetMetadata(string id) {
        ErrorOr<AssetResponse> assetResponse = await mediator.Send(new GetAssetMetadataQuery(id));

        return assetResponse.Match(
            Ok,
            Problem
        );
    }
}