using System.Collections.Generic;
using System.Threading.Tasks;
using QuestSystem.Infrastructure.MetricProviders.Playfab.Response;
using Refit;

namespace QuestSystem.Infrastructure.MetricProviders.Playfab;

public interface IPlayfabAPI
{
    [Post("/Admin/GetUserAccountInfo")]
    Task<GetUserAccountInfoResponse> GetUserAccountInfo([Body] Dictionary<string, object> bodyData);
    
    [Post("/Server/GetPlayerStatistics")]
    Task<GetPlayerStatisticsResponse> GetPlayerStatistics([Body] Dictionary<string, object> bodyData);
}
