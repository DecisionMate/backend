using Geneirodan.Abstractions.Domain;

namespace DecisionMate.Domain.Common;

public class AggregateRoot : Entity<Guid>
{
    private readonly Queue<IDomainEvent> _events = new();

    public IEnumerable<IDomainEvent> GetEvents()
    {
        while (_events.TryDequeue(out var @event))
            yield return @event;
    }
    
    protected void AddEvent(IDomainEvent @event) => _events.Enqueue(@event);
 }