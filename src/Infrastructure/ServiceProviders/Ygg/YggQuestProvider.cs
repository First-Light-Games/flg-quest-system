using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using QuestSystem.Application.Common.Interfaces.Providers;
using QuestSystem.Infrastructure.ServiceProviders.Ygg.Request;

namespace QuestSystem.Infrastructure.ServiceProviders.Ygg;

public class YggQuestProvider : IQuestProvider
{
    private readonly IYggAPI _yggAPI;
    private readonly IConfiguration _configuration;

    private List<YggQuestData> configuredQuests = new();

    public YggQuestProvider(IYggAPI yggApi,IConfiguration configuration)
    {
        _yggAPI = yggApi;
        _configuration = configuration;
    }

    public async Task SubmitQuestProgression(string userId, string questId, int progressionValue, Dictionary<string, string>? questMetadata)
    {
        var identifyUser = await _yggAPI.IdentifyUser(new YggIdentifyUserRequest(userId));

        if (!identifyUser.Success)
        {
            throw new InvalidOperationException($"Error fetching data from Ygg APIs: {identifyUser.Message}");
        }

        var issueQuestPoints = await _yggAPI.IssueQuestPoints(
            questId,
            new YggIssueQuestPointRequest(
                userId, 
                progressionValue,
                questMetadata?.GetValueOrDefault("eventDescription"),
                questMetadata?.GetValueOrDefault("eventDescription")
            )
        );
    
        if (!issueQuestPoints.Success)
        {
            throw new InvalidOperationException($"Error issuing quest points data from Ygg APIs: {identifyUser.Message}");
        }  
    }
    
}

internal record YggQuestData
{
    public string QuestId { get; init; }
    public string QuestObjective { get; init; }

    public YggQuestData(string questId, string questObjective)
    {
        QuestId = questId;
        QuestObjective = questObjective;
    }
}

