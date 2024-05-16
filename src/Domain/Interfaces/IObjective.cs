using QuestSystem.Domain.ValueObjects;

namespace QuestSystem.Domain.Interfaces
{
    public interface IObjective
    {
        string Description { get; set; }
        string Metric { get; set; }
        bool Completed { get; }
        
        bool CheckObjectiveCompletion(object metricValue); 
    }
}
