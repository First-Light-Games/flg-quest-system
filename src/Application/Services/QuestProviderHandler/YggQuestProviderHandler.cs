using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using QuestSystem.Application.Common.Interfaces.EventStream;
using QuestSystem.Application.Common.Interfaces.Providers;
using QuestSystem.Application.Common.Models;

namespace QuestSystem.Application.Services.QuestProviderHandler;

public class YggQuestProviderHandler : IEventStreamConsumer<EventStreamData>
{

    private readonly ILogger<YggQuestProviderHandler> _logger;
    private readonly IConfiguration _configuration;
    private readonly IQuestProvider _questProvider;
    
    private List<YggQuestData> _configuredQuests = new();

    public YggQuestProviderHandler(ILogger<YggQuestProviderHandler> logger, IConfiguration configuration, [FromKeyedServices(typeof(YggQuestProviderHandler))]IQuestProvider questProvider)
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
            foreach (var quest in _configuredQuests.Where(q => q.QuestStatisticName.Equals(eventData.EventName)))
            {
                _questProvider.SubmitQuestProgression(eventData.EntityId, quest.QuestId, eventData.EventNewNumericValue, null);   
            }    
        }
    }

    
    private bool IsListeningToEvent(string eventDataStatisticName)
    {
        return _configuredQuests.Any(q => q.QuestStatisticName.Equals(eventDataStatisticName));
    }

    public void ConfigureEventStreamConsumer()
    {
        // YGG has a WIP endpoint that lists all quests for a specific campaign. 
        // In the future, this can be used not only to retrieve all quests but also to preconfigure 
        // which quest objectives (statistics) this handler should manage.
        List<YggQuestData>? yggQuestDatas = _configuration.GetSection("AppSettings:QuestProvider:Ygg:quests").Get<List<YggQuestData>>();

        if (yggQuestDatas != null)
        {
            _configuredQuests = yggQuestDatas;
        }
    }
    
}

internal record YggQuestData
{
    public string QuestId { get; init; }
    public string QuestName { get; init; }
    public string QuestStatisticName { get; init; }

    public YggQuestData(string questId, string questName, string questStatisticName)
    {
        QuestId = questId;
        QuestName = questName;
        QuestStatisticName = questStatisticName;
    }
}
