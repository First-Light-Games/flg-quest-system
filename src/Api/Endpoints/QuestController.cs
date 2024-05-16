using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using QuestSystem.Application.Quests.DTOs;
using QuestSystem.Application.Quests.Queries.CheckPredictableQuestCompletion;
using QuestSystem.Application.Quests.Queries.CheckQuestCompletion;
using QuestSystem.Application.Quests.Queries.ConfigureAndCheckQuestCompletion;

namespace QuestSystem.Api.Endpoints;

public class QuestController : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapPost(ConfigureAndCheckQuestCompletion, "/configure-check-quest")
            .MapPost(CheckQuestCompletion, "/check-quest")
            .MapPost(CheckPredictableQuestCompletion, "/check-predictable-quest");
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
    public Task<QuestCompletedDTO> ConfigureAndCheckQuestCompletion(ISender sender, ConfigureAndCheckQuestCompletionQuery query)
    {
        return sender.Send(query);
    }
    
    /// <summary>
    /// Handles the CheckPredictableQuestCompletionQuery by comparing snapshot and new values, and 
    /// returning a QuestCompletedDTO indicating whether the quest is completed.
    /// </summary>
    /// <param name="query">The query object containing the metric, snapshot value, and new value.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains a 
    /// QuestCompletedDTO indicating whether the quest was completed.
    /// </returns>
    /// <response code="200">Returns a QuestCompletedDTO indicating the quest completion status.</response>
    /// <response code="400">If the snapshot value and new value are of different types.</response>
    /// <response code="500">If there is any server error during the process.</response>
    /// <exception cref="ArgumentException">Thrown when the snapshot value and new value are of different types.</exception>
    public Task<QuestCompletedDTO> CheckPredictableQuestCompletion(ISender sender, CheckPredictableQuestCompletionQuery query)
    {
        return sender.Send(query);
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
    public Task<QuestCompletedDTO> CheckQuestCompletion(ISender sender, CheckQuestCompletionQuery query)
    {
        return sender.Send(query);
    }
}
