using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using QuestSystem.Application.Common.Exceptions;
using QuestSystem.Application.Common.Interfaces;
using QuestSystem.Application.Common.Models.Provider;
using Refit;

namespace QuestSystem.Infrastructure.MetricProviders.Playfab;

// ==============================================================================================
// Playfab APIs reference
// Playfab Admin - https://learn.microsoft.com/en-us/rest/api/playfab/admin/?view=playfab-rest
// Playfab Server - https://learn.microsoft.com/en-us/rest/api/playfab/server/?view=playfab-rest
// ==============================================================================================
public class PlayfabMetricProvider : IMetricProvider
{
    private readonly IPlayfabAPI _playfabApi;

    public PlayfabMetricProvider(IPlayfabAPI playfabApi)
    {
        _playfabApi = playfabApi;
    }

    public async Task<UserData> GetUserFromProvider(string userEmail)
    {
        
        var postBody = new Dictionary<string, object>()
        {
            {"Email", userEmail}
        };

        try
        {
            var response = await _playfabApi.GetUserAccountInfo(postBody);

            var userInfo = response.Data?.UserInfo;
            if (userInfo == null || string.IsNullOrEmpty(userInfo.PlayFabId))
            {
                throw new EntityNotFoundException($"User with email {userEmail} not found.");
            }

            return new UserData()
            {
                Id = userInfo.PlayFabId,
                Username = userInfo.Username,
                UserEmail = userEmail
            };
        }
        catch (ApiException apiEx)
        {
            throw new ExternalServiceException($"API error: {apiEx.Message}");
        }
        catch (Exception ex)
        {
            throw new ExternalServiceException($"Unexpected error: {ex.Message}");
        }
    }

    public async Task<MetricData> GetMetricFromUser(string requestEmail, string metricName)
    {
        try
        {
            var userData = await GetUserFromProvider(requestEmail);

            if (string.IsNullOrEmpty(userData.Id))
            {
                throw new EntityNotFoundException($"User with email {requestEmail} not found.");
            }

            var postBody = new Dictionary<string, object>
            {
                { "PlayFabId", userData.Id }
            };

            var response = await _playfabApi.GetPlayerStatistics(postBody);

            if (response.Data?.Statistics == null)
            {
                throw new EntityNotFoundException($"Statistics not found for player with email {requestEmail}");
            }

            var metricFound = response.Data.Statistics.Find(m => m.StatisticName != null && m.StatisticName.Equals(metricName));

            if (metricFound == null)
            {
                throw new EntityNotFoundException($"Metric '{metricName}' not found for player with email {requestEmail}");
            }

            return new MetricData { Name = metricFound.StatisticName, Value = metricFound.Value };
        }
        catch (EntityNotFoundException)
        {
            throw; 
        }
        catch (ApiException apiEx)
        {
            throw new ExternalServiceException($"API error: {apiEx.Message}");
        }
        catch (Exception ex)
        {
            throw new ExternalServiceException($"Unexpected error: {ex.Message}");
        }
    }
}
