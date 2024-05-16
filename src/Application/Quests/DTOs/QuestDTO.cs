using QuestSystem.Domain.Enums;

namespace QuestSystem.Application.Quests.DTOs;

public class QuestDTO
{
    public required string Title { get; set; }
 
    public required string Description { get; set; }

    public required QuestType QuestType { get; set; }

    public required ObjectiveDTO Objective { get; set; }
}
