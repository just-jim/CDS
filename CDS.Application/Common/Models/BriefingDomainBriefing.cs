using CDS.Application.Common.Interfaces.Clients;

namespace CDS.Application.Common.Models;

public class BriefingDomainBriefing : IQueryResponse {
    public string Name { get; set; }
    public string Description { get; set; }
    public string CreatedBy { get; set; }
    public string CreatedDate { get; set; }
}