using CDS.Domain.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CDS.Infrastructure.Database.Interceptors;

public class PublishDomainEventsInterceptor(IPublisher mediator) : SaveChangesInterceptor {

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result) {
        PublishDomainEvents(eventData.Context).GetAwaiter().GetResult();
        return base.SavingChanges(eventData, result);
    }

    public async override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData, 
        InterceptionResult<int> result, 
        CancellationToken cancellationToken = default
    ) {
        await PublishDomainEvents(eventData.Context);
        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    async Task PublishDomainEvents(DbContext? dbContext) {
        if (dbContext is null) {
            return;
        }

        // Get hold of all the various entities
        List<IHasDomainEvents> entitiesWithDomainEvents = dbContext.ChangeTracker.Entries<IHasDomainEvents>()
            .Where(entry => entry.Entity.DomainEvents.Any())
            .Select(entry => entry.Entity)
            .ToList();

        // Get hold of all the various domain events
        List<IDomainEvent> domainEvents = entitiesWithDomainEvents
            .SelectMany(entity => entity.DomainEvents)
            .ToList();

        // Clear domain events
        entitiesWithDomainEvents.ForEach(entity => entity.ClearDomainEvents());

        // Publish domain events
        foreach (var domainEvent in domainEvents) {
            await mediator.Publish(domainEvent);
        }
    }
}