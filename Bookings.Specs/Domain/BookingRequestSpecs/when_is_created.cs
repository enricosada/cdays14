﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bookings.Engine.Domain.Bookings.BookingRequest;
using Bookings.Engine.Domain.Bookings.BookingRequest.Events;
using Bookings.Engine.Domain.Bookings.Resource;
using Bookings.Specs.Support;
using Machine.Specifications;

// ReSharper disable InconsistentNaming

namespace Bookings.Specs.Domain.BookingRequestSpecs
{
    public class when_is_created : in_bookingRequest_context
    {

    }

    [Subject("Given a new BookingRequest")]
    public class in_bookingRequest_context : AbstractSpecification<BookingRequestAggregate,BookingRequestState, BookingRequestId>
    {
        private static readonly BookingRequestId _id = new BookingRequestId();
        private static readonly ResourceId _resourceId = new ResourceId();

        Establish context = () => 
            SetUp();

        Because of = () => 
            Aggregate.Create(_id, 
            _resourceId, 
            new BookingTimeframe(DateTime.Parse("2014-01-01"), DateTime.Parse("2014-01-03")));

        //
        // State changes
        //
        It the_identifier_should_be_set = () =>
            State.Id.ShouldBeLike(_id);


        //
        // Events
        // 
        It BookingRequestCreated_event_should_be_raised = () =>
            LastRaisedEventOfType<BookingRequestCreated>().ShouldNotBeNull();

        It BookingRequest_event_shoud_have_the_resource_id_set = () =>
            LastRaisedEventOfType<BookingRequestCreated>()
                .BookingRequestId.ShouldBeLike(_id);

        private It BookingRequest_event_should_have_the_timeframe_set = () =>
        {
            var evt = LastRaisedEventOfType<BookingRequestCreated>();
            evt.Timeframe.From.ShouldBeLike(DateTime.Parse("2014-01-01"));
            evt.Timeframe.To.ShouldBeLike(DateTime.Parse("2014-01-03"));
        };

        //
        // Invariants
        //
        It invariants_are_satisfied = () =>
            Aggregate.EnsureAllInvariants();
    }
}