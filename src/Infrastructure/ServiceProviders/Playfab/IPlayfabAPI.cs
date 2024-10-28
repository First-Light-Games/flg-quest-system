using System.Collections.Generic;
using System.Threading.Tasks;
using QuestSystem.Infrastructure.ServiceProviders.Playfab.Response;
using Refit;

namespace QuestSystem.Infrastructure.ServiceProviders.Playfab;

public interface IPlayfabAPI
{
    [Post("/Admin/GetUserAccountInfo")]
    Task<GetUserAccountInfoResponse> GetUserAccountInfo([Body] Dictionary<string, object> bodyData);
    
    [Post("/Server/GetPlayerStatistics")]
    Task<GetPlayerStatisticsResponse> GetPlayerStatistics([Body] Dictionary<string, object> bodyData);
}
