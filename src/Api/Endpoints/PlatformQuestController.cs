using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using QuestSystem.Application.Common.Exceptions;
using QuestSystem.Application.Quests.Commands.ConfigurePlatformQuest;
using QuestSystem.Application.Quests.DTOs;
using QuestSystem.Application.Quests.Queries.CheckPlatformQuestCompletion;

namespace QuestSystem.Api.Endpoints;

public class PlatformQuestController : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapPost(ConfigurePlatformQuest, "/configure-platform-quest")
            .MapPost(CheckPlatformQuestCompletion, "/check-platform-quest-completion")
            .MapPost(CheckQuestCompletionFromZealy, "/check-zealy-quest-completion");
    }
    
    /// <summary>
    /// Handles the ConfigureAndCheckQuestCompletionQuery by configuring a quest, 
    /// checking the player's metric data, and returning a QuestCompletedDTO 
    /// indicating whether the quest is completed.
    /// </summary>
    /// <param name="query">The query object containing the quest details and the player's email.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains a 
    /// QuestCompletedDTO indicating whether the quest was completed.
    /// </returns>
    /// <response code="200">Returns a QuestCompletedDTO indicating the quest completion status.</response>
    /// <response code="400">If the player data could not be found or is null.</response>
    /// <response code="400">If the player metric data could not be found or is null.</response>
    /// <response code="500">If there is any server error during the process.</response>
    /// <exception cref="NullReferenceException">Thrown when the player's metric data could not be found.</exception>
    public Task<PlatformQuestEventKeyDTO> ConfigurePlatformQuest(ISender sender, ConfigurePlatformQuestCommand command)
    {
        return sender.Send(command);
    }
    
    
    /// <summary>
    /// Handles the CheckQuestCompletionQuery by checking the player's metric data and 
    /// returning a QuestCompletedDTO indicating whether the quest is completed.
    /// </summary>
    /// <param name="query">The query object containing the player's identifier and the quest name.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains a 
    /// QuestCompletedDTO indicating whether the quest was completed.
    /// </returns>
    /// <response code="200">Returns a QuestCompletedDTO indicating the quest completion status.</response>
    /// <response code="400">If the player data could not be found or is null.</response>
    /// <response code="400">If the player metric data could not be found or is null.</response>
    /// <response code="500">If there is any server error during the process.</response>
    /// <exception cref="InvalidOperationException">Thrown when the player's metric data is null or invalid.</exception>
    public Task<QuestCompletedDTO> CheckPlatformQuestCompletion(ISender sender, CheckPlatformQuestCompletionQuery query)
    {
        return sender.Send(query);
    }
    
    
    /// <summary>
    /// Handles the CheckQuestCompletionFromZealy by checking the player's metric data and 
    /// returning a QuestCompletedDTO indicating whether the quest is completed.
    /// </summary>
    /// <param name="query">The Zealy's payload containg the way player is identified inside the Platform</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains a 
    /// QuestCompletedDTO indicating whether the quest was completed.
    /// </returns>
    /// <response code="200">Returns a QuestCompletedDTO indicating the quest completion status.</response>
    /// <response code="400">If the player data could not be found or is null.</response>
    /// <response code="400">If the player metric data could not be found or is null.</response>
    /// <response code="500">If there is any server error during the process.</response>
    /// <exception cref="InvalidOperationException">Thrown when the player's metric data is null or invalid.</exception>
    public async Task<QuestCompletedDTO> CheckQuestCompletionFromZealy(ISender sender, HttpRequest httpRequest)
    {
        const string ApiKeyHeaderName = "X-Api-Key";
        
        if (!httpRequest.Headers.TryGetValue(ApiKeyHeaderName, out var apiKey))
        {
            throw new UnauthorizedAccessException("ApiKey not supplied/not found!");
        }

        string body;
        using (var reader = new StreamReader(httpRequest.Body, Encoding.UTF8))
        {
            body = await reader.ReadToEndAsync();
        }

        using (JsonDocument doc = JsonDocument.Parse(body))
        {
            JsonElement root = doc.RootElement;
            if (root.TryGetProperty("accounts", out JsonElement accounts) && 
                accounts.TryGetProperty("email", out JsonElement email))
            {
                var checkResult = await sender.Send(new CheckPlatformQuestCompletionQuery
                {
                    PlatformQuestEventKey = apiKey.ToString(),
                    PlayerIdentifier = "magostinhojr@gmail.com" //email.GetString() ?? throw new InvalidOperationException("Email cannot be null")
                });

                if (!checkResult.Completed)
                {
                    throw new PlatformQuestNotCompletedException("The user didn't complete the quest");
                }

                return checkResult;
            }

            throw new ArgumentException("Invalid JSON structure from Zealy: Missing Player Identifier");
        }
    }
}
