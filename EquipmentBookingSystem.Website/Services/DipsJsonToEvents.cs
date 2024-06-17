using EquipmentBookingSystem.Website.Models;
using Newtonsoft.Json.Linq;

namespace EquipmentBookingSystem.Website.Services;

public class DipsJsonToEvents
{

    public IEnumerable<Event> ConvertJsonToEvents(string json)
    {
        if (json is null)
        {
            throw new ArgumentNullException(json);
        }

        var events = new List<Event>();
        var jObject = JObject.Parse(json);
        foreach (var eventObject in jObject)
        {
            var eventJsonObject = eventObject.Value as JObject;
            if (eventJsonObject is null)
            {
                continue;
            }

            var newEvent = new Event
            {
                EventCoverStart = DateTime.SpecifyKind(DateTime.Parse(eventJsonObject["startDateTime"]?.ToString()), DateTimeKind.Utc),
                EventCoverEnd = DateTime.SpecifyKind(DateTime.Parse(eventJsonObject["endDateTime"]?.ToString()), DateTimeKind.Utc),
                DipsEventLocationString = eventJsonObject["Event Location"]?.ToString(),
                DipsEventPostcode = eventJsonObject["Event Postcode"]?.ToString(),
                DipsEventType = eventJsonObject["Event Type"]?.ToString(),
                DipsIsDeleted = eventJsonObject["Is Deleted"]?.Value<bool>() ?? false,
                DipsId = eventJsonObject["SJA Event ID"]?.ToString(),
                DipsEventTitle = eventJsonObject["Event Title"]?.ToString() ?? string.Empty,
                DipsAllocatedArea = eventJsonObject["Allocated Area"]?.ToString(),
                DipsAllocatedDistrict = eventJsonObject["Allocated District"]?.ToString(),
            };

            events.Add(newEvent);
        }

        return events;
    }
}
