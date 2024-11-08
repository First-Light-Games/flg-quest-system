using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuestSystem.Application.Common.Interfaces.Providers;

/// <summary>
/// Interface for submitting quest progression data to third-party quest provider systems.
/// </summary>
public interface IQuestProvider<T>
{
    /// <summary>
    /// Submits the progression value for a specified quest to the third-party provider associated with a user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user progressing through the quest.</param>
    /// <param name="questId">The unique identifier of the quest being progressed.</param>
    /// <param name="questMetadata">Additional metadata related to the quest, such as context or status indicators.</param>
    /// <param name="progressionValue">The numeric value representing the user's current progression in the quest.</param>
    /// <returns>A task representing the asynchronous operation of submitting the quest progression to the third-party system.</returns>
    Task SubmitQuestProgression(string userId, string questId, int questProgressionValue, Dictionary<string, string>? questMetadata);

    Task SetupActiveQuests();

    List<T> GetActiveQuests();
}
