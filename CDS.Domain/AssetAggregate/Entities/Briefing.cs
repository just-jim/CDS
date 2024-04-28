using CDS.Domain.AssetAggregate.ValueObjects;
using CDS.Domain.Common.Models;

namespace CDS.Domain.AssetAggregate.Entities;

public sealed class Briefing : Entity<BriefingId> {
    public string CreatedBy { get; private set; }
    public string CreatedDate { get; private set; }

    Briefing(string createdBy, string createdDate)
        : base(BriefingId.CreateUnique()) {
        CreatedBy = createdBy;
        CreatedDate = createdDate;
    }

    public static Briefing Create(string createdBy, string createdDate) {
        return new Briefing(createdBy, createdDate);
    }
}