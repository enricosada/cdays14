﻿using System;
using Bookings.Engine.Domain.Auth.Users;
using Bookings.Engine.Domain.Bookings.BookingRequest;
using Bookings.Engine.Domain.Bookings.Resource.Events;
using Bookings.Engine.Support;

namespace Bookings.Engine.Domain.Bookings.Resource
{
    public class ResourceAggregate : Aggregate<ResourceState, ResourceId>
    {
        public ResourceAggregate(ResourceState state = null)
            : base(state ?? new ResourceState())
        {
        }

        public ResourceAggregate()
            : this(null)
        {
        }

        public void Create(ResourceId id, ResourceName name, UserId managerId)
        {
            if (id == null)
                throw new ArgumentNullException("id");

            if (name == null)
                throw new ArgumentNullException("name");

            if (managerId == null)
                throw new ArgumentNullException("managerId");

            RaiseEvent(new ResourceCreated(id, name));
            RaiseEvent(new ResourceManagerAdded(managerId));
        }

        public void Book(
            BookingRequestId requestId,
            UserId approvedByUserId,
            BookingInterval interval
        )
        {
            if (!State.IsManager(approvedByUserId))
            {

                return;
            }

            if (State.IsResourceAvailable(interval))
            {
                RaiseEvent(new ResourceBooked(requestId, approvedByUserId, interval));
            }
            else
            {
                RaiseEvent(new ResourceBookingFailed(requestId, "Unavailable"));
            }
        }
    }
}
