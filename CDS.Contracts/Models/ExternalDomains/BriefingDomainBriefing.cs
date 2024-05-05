using CDS.Contracts.Interfaces.Clients;

namespace CDS.Contracts.Models.ExternalDomains;

public class BriefingDomainBriefing : IQueryResponse {
    public string Name { get; set; }
    public string Description { get; set; }
    public string CreatedBy { get; set; }
    public string CreatedDate { get; set; }

    public BriefingDomainBriefing(string name, string description, string createdBy, string createdDate) {
        Name = name;
        Description = description;
        CreatedBy = createdBy;
        CreatedDate = createdDate;
    }
}