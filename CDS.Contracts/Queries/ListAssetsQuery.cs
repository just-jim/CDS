using CDS.Contracts.Models.Responses;
using ErrorOr;
using MediatR;

namespace CDS.Contracts.Queries;

public class ListAssetsQuery : IRequest<ErrorOr<List<AssetShortResponse>>> {
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}