using Microsoft.AspNetCore.Builder;

namespace QuestSystem.Api.Infrastructure;

public abstract class EndpointGroupBase
{
    public abstract void Map(WebApplication app);
}
