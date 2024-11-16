using Newtonsoft.Json;

namespace QuestSystem.Infrastructure.ServiceProviders.Ygg.Request;

public class YggIdentifyUserRequest
{
    public string Email { get; set; }

    public YggIdentifyUserRequest(string email)
    {
        Email = email;
    }
}
