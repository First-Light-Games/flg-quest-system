namespace QuestSystem.Application.Common.Models;

public class EventStreamData
{
    public string EntityId;
    public string EventName;
    public string? EventDescription;
    
    public string StatisticName;
    public int StatisticPreviousNumericValue;
    public int StatisticNewNumericValue;

    public EventStreamData(string entityId, string eventName, string statisticName, int statisticPreviousNumericValue, int statisticNewNumericValue)
    {
        EntityId = entityId;
        EventName = eventName;
        StatisticName = statisticName;
        StatisticPreviousNumericValue = statisticPreviousNumericValue;
        StatisticNewNumericValue = statisticNewNumericValue;
    }
}
