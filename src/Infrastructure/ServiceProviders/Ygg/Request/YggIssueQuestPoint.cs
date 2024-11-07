namespace QuestSystem.Infrastructure.ServiceProviders.Ygg.Request;

public class YggIssueQuestPointRequest
{
    private string _userEmail;
    private int _questPoints;
    private string? _eventName;
    private string? _eventDescription;

    public YggIssueQuestPointRequest(string userEmail, int questPoints, string? eventName, string? eventDescription)
    {
        _userEmail = userEmail;
        _questPoints = questPoints;
        _eventName = eventName;
        _eventDescription = eventDescription;
    }
}
