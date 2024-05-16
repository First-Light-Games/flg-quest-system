namespace QuestSystem.Application.Quests.DTOs;

public class ObjectiveDTO
{
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required string Metric { get; set; }
    public object? Goal { get; set; }
}
