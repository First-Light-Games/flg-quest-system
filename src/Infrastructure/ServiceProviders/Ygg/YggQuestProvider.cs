using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using QuestSystem.Application.Common.Interfaces.Providers;
using QuestSystem.Infrastructure.ServiceProviders.Ygg.Request;

namespace QuestSystem.Infrastructure.ServiceProviders.Ygg;

public class YggQuestProvider : IQuestProvider
{
    private readonly IYggAPI _yggAPI;

    public YggQuestProvider(IYggAPI yggApi)
    {
        _yggAPI = yggApi;
    }
    
    public async Task SubmitQuestProgression(string userId, string questId, int progressionValue, Dictionary<string, string> questMetadata)
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
                questMetadata.GetValueOrDefault("eventName"),
                questMetadata.GetValueOrDefault("eventDescription")
                )
            );
        
        if (!issueQuestPoints.Success)
        {
            throw new InvalidOperationException($"Error issuing quest points data from Ygg APIs: {identifyUser.Message}");
        }

    }
}
