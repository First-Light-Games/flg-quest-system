using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuestSystem.Application.Common.Interfaces.Providers;

public interface IQuestProvider
{
    Task SubmitQuestProgression(string userId, string questId, int progressionValue,
        Dictionary<string, string> questMetadata);
}
