namespace QuestSystem.Application.Common.Models;

public class EventStreamData
{
    public string EntityId;
    public string EventName;
    
    public int EventPreviousNumericValue;
    public int EventNewNumericValue;

    public EventStreamData(string entityId, string eventName, int eventPreviousNumericValue, int eventNewNumericValue)
    {
        EntityId = entityId;
        EventName = eventName;
        EventPreviousNumericValue = eventPreviousNumericValue;
        EventNewNumericValue = eventNewNumericValue;
    }
}
