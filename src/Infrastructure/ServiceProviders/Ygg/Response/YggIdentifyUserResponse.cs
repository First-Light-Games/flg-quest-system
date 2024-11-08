namespace QuestSystem.Infrastructure.ServiceProviders.Ygg.Response;

public class YggIdentifyUserResponse : YggBaseResponse
{
    public YggUserData Data { get; set; } = new();
}

public class YggUserData
{
    public string? yggUserId { get; set; } = null!;
}
