using System;
using EquipmentBookingSystem.Website.Models;

namespace EquipmentBookingSystem.Website.Test.Models;

public class EventTest
{
    [Fact]
    public void Test1()
    {
        // Arrange
        var event1 = new Event
        {
            Id = Guid.NewGuid(),
            EventCoverStart = DateTime.Now,
            EventCoverEnd = DateTime.Now,
            EventStart = DateTime.Now,
            EventEnd = DateTime.Now,
            DipsEventLocationString = "DipsEventLocationString",
            DipsEventPostcode = "DipsEventPostcode",
            DipsId = "DipsId",
            DipsEventTitle = "DipsEventTitle",
            DipsEventType = "DipsEventType",
            DipsIsDeleted = false,
            DipsAllocatedArea =  "DipsAllocatedArea",
            DipsAllocatedDistrict = "DipsAllocatedDistrict"
        };

        // Act

        // Assert
        Assert.NotNull(event1);
        Assert.NotEqual(Guid.Empty, event1.Id);
        Assert.Equal(DateTime.Now.Date, event1.EventCoverStart.Value.Date);
        Assert.Equal(DateTime.Now.Date, event1.EventCoverEnd.Value.Date);
        Assert.Equal(DateTime.Now.Date, event1.EventStart.Value.Date);
        Assert.Equal(DateTime.Now.Date, event1.EventEnd.Value.Date);
        Assert.Equal("DipsEventLocationString", event1.DipsEventLocationString);
        Assert.Equal("DipsEventPostcode", event1.DipsEventPostcode);
        Assert.Equal("DipsId", event1.DipsId);
        Assert.Equal("DipsEventTitle", event1.DipsEventTitle);
        Assert.Equal("DipsEventType", event1.DipsEventType);
        Assert.False(event1.DipsIsDeleted);
        Assert.Equal("DipsAllocatedArea", event1.DipsAllocatedArea);
        Assert.Equal("DipsAllocatedDistrict", event1.DipsAllocatedDistrict);
    }
}
