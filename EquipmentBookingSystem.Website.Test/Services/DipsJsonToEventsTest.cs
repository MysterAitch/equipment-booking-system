using System;
using System.Linq;
using EquipmentBookingSystem.Website.Services;
using Newtonsoft.Json;

namespace EquipmentBookingSystem.Website.Test.Services;

public class DipsJsonToEventsTest
{
    private const string JsonInput = """
                                     {
                                       "964335": {
                                         "Is Deleted": false,
                                         "Event Type": "Event Cover",
                                         "SJA Event ID": "964335",
                                         "Date": "2024/06/01",
                                         "Event Title": "WWMD - Event name 123 - ZEF004",
                                         "Event Location": "Address line 1, Address line 2, Address line 3, , A12 3CD",
                                         "Event Postcode": "A12 3CD",
                                         "Start Time": "12:00",
                                         "Finish Time": "23:00",
                                         "startDateTime": "2024-06-01T11:00:00.000Z",
                                         "endDateTime": "2024-06-01T22:00:00.000Z",
                                         "requirements": {
                                           "CDT": 0,
                                           "TFA": 0,
                                           "FA": 9,
                                           "AFA": 2,
                                           "FIT": 0,
                                           "PTA": 0,
                                           "ETA": 2,
                                           "EMT": 0,
                                           "PAR": 0,
                                           "NUR": 0,
                                           "DOC": 0,
                                           "COM": 1,
                                           "DO": 1,
                                           "SM": 0,
                                           "CRU": 0
                                         },
                                         "Allocated Area": "West Midlands District",
                                         "Allocated District": "West Midlands District"
                                       }
                                     }
                                     """;

    [Fact]
    public void Should_Produce_Correct_Number_Of_Events()
    {
        var converter = new DipsJsonToEvents();
        var events = converter.ConvertJsonToEvents(JsonInput);

        Assert.Equal(1, events.Count());
    }

    [Fact]
    public void Should_Produce_Events_With_Correct_Details()
    {
        var converter = new DipsJsonToEvents();
        var events = converter.ConvertJsonToEvents(JsonInput).ToList();

        var firstEvent = events[0];
        Assert.Equal("964335", firstEvent.DipsId);
        Assert.Equal("Event Cover", firstEvent.DipsEventType);
        Assert.False(firstEvent.DipsIsDeleted);
        Assert.Equal("WWMD - Event name 123 - ZEF004", firstEvent.DipsEventTitle);
        Assert.Equal("West Midlands District", firstEvent.DipsAllocatedArea);
        Assert.Equal("West Midlands District", firstEvent.DipsAllocatedDistrict);

        // UK London timezone
        var timezone = TimeZoneInfo.FindSystemTimeZoneById("Europe/London");

        Assert.Equal(
            TimeZoneInfo.ConvertTime(
                DateTime.SpecifyKind(new DateTime(2024, 6, 1, 11, 0, 0), DateTimeKind.Utc),
                timezone
            ),
            firstEvent.EventCoverStart
        );
        Assert.Equal(
            TimeZoneInfo.ConvertTime(
                DateTime.SpecifyKind(new DateTime(2024, 6, 1, 22, 0, 0), DateTimeKind.Utc),
                timezone),
            firstEvent.EventCoverEnd
        );

        Assert.Equal(
            "Address line 1, Address line 2, Address line 3, , A12 3CD",
            firstEvent.DipsEventLocationString
        );
        Assert.Equal("A12 3CD", firstEvent.DipsEventPostcode);
    }

    [Fact]
    public void Should_Handle_Null_Json_Input()
    {
        var converter = new DipsJsonToEvents();
        Assert.Throws<System.ArgumentNullException>(() => converter.ConvertJsonToEvents(null));
    }

    [Fact]
    public void Should_Handle_Empty_Json_Input()
    {
        var converter = new DipsJsonToEvents();
        Assert.Throws<JsonReaderException>(() => converter.ConvertJsonToEvents(""));
    }
}
