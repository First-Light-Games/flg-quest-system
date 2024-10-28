namespace QuestSystem.Infrastructure.ServiceProviders.Ygg.Request;

public class YggIssueQuestPointRequest
{
    private string _userId;
    private int _progressionValue;
    private string? _eventName;
    private string? _eventDescription;

    public YggIssueQuestPointRequest(string userId, int progressionValue, string? eventName, string? eventDescription)
    {
        _userId = userId;
        _progressionValue = progressionValue;
        _eventName = eventName;
        _eventDescription = eventDescription;
    }
}
