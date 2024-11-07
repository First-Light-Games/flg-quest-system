using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using QuestSystem.Application.Common.Interfaces.EventStream;
using QuestSystem.Application.Common.Interfaces.Providers;
using QuestSystem.Application.Common.Models;

namespace QuestSystem.Application.Services.QuestProviderHandler;

public class YggQuestProviderHandler : IEventStreamConsumer<EventStreamData>
{

    private readonly ILogger<YggQuestProviderHandler> _logger;
    private readonly IConfiguration _configuration;
    private readonly IQuestProvider<YggQuest> _questProvider;
    
    private List<YggQuest> _configuredQuests = new();

    public YggQuestProviderHandler(ILogger<YggQuestProviderHandler> logger, IConfiguration configuration, IQuestProvider<YggQuest> questProvider)
    {
        _logger = logger;
        _configuration = configuration;
        _questProvider = questProvider;
        
        ConfigureEventStreamConsumer();
    }
    
    
    public void OnEventReceived(EventStreamData eventData)
    {
        if (IsListeningToEvent(eventData.EventName))
        {
            foreach (var quest in _configuredQuests.Where(q => q.QuestMetricName.Equals(eventData.EventName)))
            {
                _questProvider.SubmitQuestProgression(eventData.EntityId, quest.QuestId, eventData.EventNewNumericValue, null);   
            }    
        }
    }

    
    private bool IsListeningToEvent(string eventDataStatisticName)
    {
        return _configuredQuests.Any(q => q.QuestMetricName.Equals(eventDataStatisticName));
    }

    public void ConfigureEventStreamConsumer()
    {
        _configuredQuests = _questProvider.GetActiveQuests();
    }
    
}

public record YggQuest
{
    public string QuestId { get; init; }
    public string QuestName { get; init; }
    public string QuestMetricName { get; init; }
    public int QuestPoints { get; set; }
    
    public YggQuest(string questId, string questName, string questMetricName, int questPoints)
    {
        QuestId = questId;
        QuestName = questName;
        QuestMetricName = questMetricName;
        QuestPoints = questPoints;
    }
}
