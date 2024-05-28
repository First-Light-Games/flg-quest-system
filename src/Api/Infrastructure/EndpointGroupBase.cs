using Microsoft.AspNetCore.Builder;

namespace QuestSystem.Web.Infrastructure;

public abstract class EndpointGroupBase
{
    public abstract void Map(WebApplication app);
}
