namespace QuestSystem.Infrastructure.ServiceProviders.Playfab.Response;

public class GetUserAccountInfoResponse
{
    public UserAccountInfoData? Data { get; set; }
}

public class UserAccountInfoData
{
    public UserInfo? UserInfo { get; set; }
}

public class UserInfo
{
    public string? PlayFabId { get; set; }
    public string? Username { get; set; }
    public PrivateInfo? PrivateInfo { get; set; }
}

public class PrivateInfo
{
    public string? Email;
}
