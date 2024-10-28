using System.Collections.Generic;
using System.Threading.Tasks;
using QuestSystem.Infrastructure.ServiceProviders.Ygg.Request;
using QuestSystem.Infrastructure.ServiceProviders.Ygg.Response;
using Refit;

namespace QuestSystem.Infrastructure.ServiceProviders.Ygg;

public interface IYggAPI
{
    
    //API Reference: https://api-docs.yieldguild.io/docs/users/Identify
    [Post("/users/identify")]
    Task<YggIdentifyUserResponse> IdentifyUser([Body] YggIdentifyUserRequest bodyData);
    
    //API Reference: https://api-docs.yieldguild.io/docs/group-quests/IssueQuestPoints
    [Post("/group-quests/{questId}/points")]
    Task<YggIssueQuestPointsResponse> IssueQuestPoints(string questId, [Body] YggIssueQuestPointRequest bodyData);
    
    //API Reference: https://api-docs.yieldguild.io/docs/group-quests/GetGroupQuests
    [Get("/group-quests")]
    Task<YggGetQuestsResponse> GetQuests();
    
    //API Reference: https://api-docs.yieldguild.io/docs/group-quests/GetGroupQuests
    [Get("/group-quests/{questId}/enrolled-users/{yggUserId}")]
    Task<YggGetQuestsResponse> GetEnrolledUser(string questId, string yggUserId);
    
}
