using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using QuestSystem.Application.Common.Interfaces.Providers;
using QuestSystem.Application.Services.QuestProviderHandler;
using QuestSystem.Infrastructure.ServiceProviders.Playfab;
using QuestSystem.Infrastructure.ServiceProviders.RemoteConfig;
using QuestSystem.Infrastructure.ServiceProviders.Ygg.Request;

namespace QuestSystem.Infrastructure.ServiceProviders.Ygg;

public class YggQuestProvider : IQuestProvider<YggQuest>
{
    private ILogger<YggQuestProvider> _logger;
    
    private readonly IYggAPI _yggAPI;
    private readonly IPlayfabAPI _playfabApi;
    private readonly IRemoteConfigAPI _remoteConfigApi;
    private readonly IConfiguration _configuration;

    private readonly List<YggQuest> _activeQuests = [];

    public YggQuestProvider(IYggAPI yggApi,IConfiguration configuration, IPlayfabAPI playfabApi, IRemoteConfigAPI remoteConfigApi, ILogger<YggQuestProvider> logger)
    {
        _yggAPI = yggApi;
        _configuration = configuration;
        _playfabApi = playfabApi;
        _remoteConfigApi = remoteConfigApi;
        _logger = logger;
    }

    public async Task SubmitQuestProgression(string userId, string questId, int progressionValue, Dictionary<string, string>? questMetadata)
    {
        var userAccountInfo = await _playfabApi.GetUserAccountInfo(new Dictionary<string, object>()
        {
            { "PlayFabId", userId }
        });

        if (!string.IsNullOrEmpty(userAccountInfo.Data?.UserInfo?.PrivateInfo?.Email))
        {
            var email = userAccountInfo.Data.UserInfo.PrivateInfo.Email;

            var issueQuestPoints = await _yggAPI.IssueQuestPoints(
                questId,
                new YggIssueQuestPointRequest(
                    email, 
                    progressionValue,
                    questMetadata?.GetValueOrDefault("eventDescription"),
                    questMetadata?.GetValueOrDefault("eventDescription")
                )
            );
            
        }
    }

    public async Task SetupActiveQuests()
    {
        // YGG has a WIP endpoint that lists all quests for a specific campaign. 
        // In the future, this can be used not only to retrieve all quests but also to preconfigure 
        // which quest objectives (statistics) this handler should manage.
        var remoteConfigPath = _configuration.GetValue<string>("AppSettings:QuestProvider:Ygg:QuestConfig");
        
        if (remoteConfigPath != null)
        {
            var questConfig = await _remoteConfigApi.GetJsonRemoteConfig(remoteConfigPath);
        
            if (questConfig.ValueKind == JsonValueKind.Array && questConfig.GetArrayLength() > 0)
            {
                foreach (var quest in questConfig.EnumerateArray())
                {
                    ConfigureQuest(quest);
                }
            }
            else if (questConfig.ValueKind == JsonValueKind.Object && questConfig.EnumerateObject().Any())
            {
                ConfigureQuest(questConfig);
            }
            else
            {
                throw new InvalidOperationException($"Error parsing RemoteConfig YggQuest for element {questConfig}");
            }
        }
    }

    public List<YggQuest> GetActiveQuests()
    {
        return _activeQuests;
    }

    private void ConfigureQuest(JsonElement quest)
    {
        try
        {
            var yggQuest = JsonSerializer.Deserialize<YggQuest>(quest.GetRawText());

            if (yggQuest != null)
            {
                _activeQuests.Add(yggQuest);
            }
        }
        catch (ArgumentException ex)
        {
            _logger.LogError($"Error while trying to parse YggQuestData from RemoteConfig: {ex.Message}");
            throw;
        }
    }
    

}


