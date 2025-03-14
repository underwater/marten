// <auto-generated/>
#pragma warning disable
using Marten;
using Marten.Events.Aggregation;
using Marten.Internal.Storage;
using Marten.Storage;
using System;
using System.Linq;

namespace Marten.Generated.EventStore
{
    // START: AggregateProjectionLiveAggregation973344970
    public class AggregateProjectionLiveAggregation973344970 : Marten.Events.Aggregation.SyncLiveAggregatorBase<EventSourcingTests.Bugs.Bug_2025_event_inheritance_in_projection.Identity>
    {
        private readonly Marten.Events.Aggregation.SingleStreamAggregation<EventSourcingTests.Bugs.Bug_2025_event_inheritance_in_projection.Identity> _singleStreamAggregation;

        public AggregateProjectionLiveAggregation973344970(Marten.Events.Aggregation.SingleStreamAggregation<EventSourcingTests.Bugs.Bug_2025_event_inheritance_in_projection.Identity> singleStreamAggregation)
        {
            _singleStreamAggregation = singleStreamAggregation;
        }



        public override EventSourcingTests.Bugs.Bug_2025_event_inheritance_in_projection.Identity Build(System.Collections.Generic.IReadOnlyList<Marten.Events.IEvent> events, Marten.IQuerySession session, EventSourcingTests.Bugs.Bug_2025_event_inheritance_in_projection.Identity snapshot)
        {
            if (!events.Any()) return null;
            EventSourcingTests.Bugs.Bug_2025_event_inheritance_in_projection.Identity identity = null;
            snapshot ??= Create(events[0], session);
            foreach (var @event in events)
            {
                snapshot = Apply(@event, snapshot, session);
            }

            return snapshot;
        }


        public EventSourcingTests.Bugs.Bug_2025_event_inheritance_in_projection.Identity Create(Marten.Events.IEvent @event, Marten.IQuerySession session)
        {
            switch (@event)
            {
                case Marten.Events.IEvent<EventSourcingTests.Bugs.Bug_2025_event_inheritance_in_projection.UserCreated> event_UserCreated107:
                    return new EventSourcingTests.Bugs.Bug_2025_event_inheritance_in_projection.Identity(event_UserCreated107.Data);
                    break;
            }

            throw new System.InvalidOperationException("There is no default constructor for EventSourcingTests.Bugs.Bug_2025_event_inheritance_in_projection.Identity");
        }


        public EventSourcingTests.Bugs.Bug_2025_event_inheritance_in_projection.Identity Apply(Marten.Events.IEvent @event, EventSourcingTests.Bugs.Bug_2025_event_inheritance_in_projection.Identity aggregate, Marten.IQuerySession session)
        {
            switch (@event)
            {
                case Marten.Events.IEvent<EventSourcingTests.Bugs.Bug_2025_event_inheritance_in_projection.IdentityAdded> event_IdentityAdded108:
                    aggregate.Apply(event_IdentityAdded108.Data);
                    break;
            }

            return aggregate;
        }

    }

    // END: AggregateProjectionLiveAggregation973344970


    // START: AggregateProjectionInlineHandler973344970
    public class AggregateProjectionInlineHandler973344970 : Marten.Events.Aggregation.AggregationRuntime<EventSourcingTests.Bugs.Bug_2025_event_inheritance_in_projection.Identity, System.Guid>
    {
        private readonly Marten.IDocumentStore _store;
        private readonly Marten.Events.Aggregation.IAggregateProjection _projection;
        private readonly Marten.Events.Aggregation.IEventSlicer<EventSourcingTests.Bugs.Bug_2025_event_inheritance_in_projection.Identity, System.Guid> _slicer;
        private readonly Marten.Storage.ITenancy _tenancy;
        private readonly Marten.Internal.Storage.IDocumentStorage<EventSourcingTests.Bugs.Bug_2025_event_inheritance_in_projection.Identity, System.Guid> _storage;
        private readonly Marten.Events.Aggregation.SingleStreamAggregation<EventSourcingTests.Bugs.Bug_2025_event_inheritance_in_projection.Identity> _singleStreamAggregation;

        public AggregateProjectionInlineHandler973344970(Marten.IDocumentStore store, Marten.Events.Aggregation.IAggregateProjection projection, Marten.Events.Aggregation.IEventSlicer<EventSourcingTests.Bugs.Bug_2025_event_inheritance_in_projection.Identity, System.Guid> slicer, Marten.Storage.ITenancy tenancy, Marten.Internal.Storage.IDocumentStorage<EventSourcingTests.Bugs.Bug_2025_event_inheritance_in_projection.Identity, System.Guid> storage, Marten.Events.Aggregation.SingleStreamAggregation<EventSourcingTests.Bugs.Bug_2025_event_inheritance_in_projection.Identity> singleStreamAggregation) : base(store, projection, slicer, storage)
        {
            _store = store;
            _projection = projection;
            _slicer = slicer;
            _tenancy = tenancy;
            _storage = storage;
            _singleStreamAggregation = singleStreamAggregation;
        }



        public override async System.Threading.Tasks.ValueTask<EventSourcingTests.Bugs.Bug_2025_event_inheritance_in_projection.Identity> ApplyEvent(Marten.IQuerySession session, Marten.Events.Projections.EventSlice<EventSourcingTests.Bugs.Bug_2025_event_inheritance_in_projection.Identity, System.Guid> slice, Marten.Events.IEvent evt, EventSourcingTests.Bugs.Bug_2025_event_inheritance_in_projection.Identity aggregate, System.Threading.CancellationToken cancellationToken)
        {
            switch (evt)
            {
                case Marten.Events.IEvent<EventSourcingTests.Bugs.Bug_2025_event_inheritance_in_projection.UserCreated> event_UserCreated111:
                    aggregate ??= new EventSourcingTests.Bugs.Bug_2025_event_inheritance_in_projection.Identity(event_UserCreated111.Data);
                    return aggregate;
                case Marten.Events.IEvent<EventSourcingTests.Bugs.Bug_2025_event_inheritance_in_projection.IdentityAdded> event_IdentityAdded110:
                    if(aggregate == default) throw new ArgumentException("Projection for EventSourcingTests.Bugs.Bug_2025_event_inheritance_in_projection+Identity should either have the Create Method or Constructor for event of type Marten.Events.IEvent<EventSourcingTests.Bugs.Bug_2025_event_inheritance_in_projection.IdentityAdded>, or EventSourcingTests.Bugs.Bug_2025_event_inheritance_in_projection+Identity should have a Default Constructor.");
                    aggregate.Apply(event_IdentityAdded110.Data);
                    return aggregate;
            }

            return aggregate;
        }


        public EventSourcingTests.Bugs.Bug_2025_event_inheritance_in_projection.Identity Create(Marten.Events.IEvent @event, Marten.IQuerySession session)
        {
            switch (@event)
            {
                case Marten.Events.IEvent<EventSourcingTests.Bugs.Bug_2025_event_inheritance_in_projection.UserCreated> event_UserCreated109:
                    return new EventSourcingTests.Bugs.Bug_2025_event_inheritance_in_projection.Identity(event_UserCreated109.Data);
                    break;
            }

            throw new System.InvalidOperationException("There is no default constructor for EventSourcingTests.Bugs.Bug_2025_event_inheritance_in_projection.Identity");
        }

    }

    // END: AggregateProjectionInlineHandler973344970


}
