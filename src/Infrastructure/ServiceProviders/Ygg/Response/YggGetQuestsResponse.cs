using System.Collections.Generic;

namespace QuestSystem.Infrastructure.ServiceProviders.Ygg.Response;

public class YggGetQuestsResponse : YggBaseResponse
{
    public List<YggQuest> Data { get; set; } = new();
}

public class YggQuest
{
    public string? questId { get; set; } = null!;
}
