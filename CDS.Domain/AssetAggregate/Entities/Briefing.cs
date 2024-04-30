using CDS.Domain.AssetAggregate.ValueObjects;
using CDS.Domain.Common.Models;

namespace CDS.Domain.AssetAggregate.Entities;

public sealed class Briefing : Entity<BriefingId> {
    public string CreatedBy { get; private set; }
    public DateOnly CreatedDate { get; private set; }

    Briefing(string createdBy, DateOnly createdDate)
        : base(BriefingId.CreateUnique()) {
        CreatedBy = createdBy;
        CreatedDate = createdDate;
    }

    public static Briefing Create(string createdBy, DateOnly createdDate) {
        return new Briefing(createdBy, createdDate);
    }
}