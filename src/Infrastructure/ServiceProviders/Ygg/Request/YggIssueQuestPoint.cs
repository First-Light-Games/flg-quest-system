using Newtonsoft.Json;
using Refit;

namespace QuestSystem.Infrastructure.ServiceProviders.Ygg.Request;

public class YggIssueQuestPointRequest
{
    
    public string? YggUserId { get; set; }
    
    public int EventPoints { get; set; }
    
    public string? EventName { get; set; }
    
    public string? EventDescription { get; set; }
    
    public YggIssueQuestPointRequest(string? yggUserId, int eventPoints, string? eventName, string? eventDescription)
    {
        YggUserId = yggUserId;
        EventPoints = eventPoints;
        EventName = eventName;
        EventDescription = eventDescription;
    }
}
