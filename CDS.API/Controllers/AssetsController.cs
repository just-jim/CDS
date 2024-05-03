using CDS.Application.Assets.Queries.GetAsset;
using CDS.Application.Assets.Queries.GetAssetMetadata;
using CDS.Application.Assets.Queries.ListAssets;
using CDS.Contracts;
using CDS.Domain.AssetAggregate;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CDS.API.Controllers;

[Route("assets/")]
public class AssetsController(ISender mediator) : ApiController
{
    
    [HttpGet]
    public async Task<IActionResult> ListAssets()
    {
        ErrorOr<List<Asset>> listAssetsResult = await mediator.Send(new ListAssetsQuery());

        return listAssetsResult.Match(
            Ok,
            Problem
        );
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsset(string id)
    {
        ErrorOr<string> listAssetsResult = await mediator.Send(new GetAssetQuery(id));

        return listAssetsResult.Match(
            Ok,
            Problem
        );
    }
    
    [HttpGet("{id}/metadata")]
    public async Task<IActionResult> GetAssetMetadata(string id)
    {
        ErrorOr<AssetResponse> assetResponse = await mediator.Send(new GetAssetMetadataQuery(id));

        return assetResponse.Match(
            Ok,
            Problem
        );
    }
}