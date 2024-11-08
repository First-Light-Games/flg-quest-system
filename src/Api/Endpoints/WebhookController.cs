using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QuestSystem.Api.Infrastructure;
using QuestSystem.Application.Common.Interfaces;
using QuestSystem.Application.Common.Interfaces.EventStream;

namespace QuestSystem.Api.Endpoints;

public class WebhookController : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup("webhook")
            .MapPost("/playfab", HandlePlayfabWebhook);
    }
    
    
    public async Task<IResult> HandlePlayfabWebhook(ISender sender, 
            [FromServices] ILogger<WebhookController> logger,
            [FromServices] IEventStreamDataProcessor<JsonElement> eventStreamDataProcessor, 
            [FromBody] JsonElement playfabPayload)
    {
        logger.LogInformation("Received PlayFab webhook event.");

        if (playfabPayload.ValueKind == JsonValueKind.Undefined)
        {
            logger.LogWarning("Received an empty or invalid payload from Playfab");
            
            return Results.BadRequest("Invalid payload");
        }
        
        await eventStreamDataProcessor.ProcessEventDataAsync(playfabPayload);
        
        return Results.Ok();
    }
}
