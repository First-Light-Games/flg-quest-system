namespace QuestSystem.Infrastructure.ServiceProviders.Ygg.Request;

public class YggIdentifyUserRequest
{
    private string _email;

    public YggIdentifyUserRequest(string email)
    {
        this._email = email;
    }
}
