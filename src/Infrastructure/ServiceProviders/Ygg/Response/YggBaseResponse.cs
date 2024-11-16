using Newtonsoft.Json;

namespace QuestSystem.Infrastructure.ServiceProviders.Ygg.Response;

public class YggBaseResponse
{
    public bool Success { get; set; }

    public string? Message { get; set; }
}

