using CDS.Contracts;
using ErrorOr;
using MediatR;

namespace CDS.Application.Assets.Queries.ListAssets;

public class ListAssetsQuery : IRequest<ErrorOr<List<AssetShortResponse>>> {
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}