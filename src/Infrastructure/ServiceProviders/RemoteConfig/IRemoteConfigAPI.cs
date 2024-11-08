using System.Text.Json;
using System.Threading.Tasks;
using Refit;

namespace QuestSystem.Infrastructure.ServiceProviders.RemoteConfig;

public interface IRemoteConfigAPI
{
    [Get("/{jsonFilename}")]
    Task<JsonElement> GetJsonRemoteConfig(string jsonFilename);
}
