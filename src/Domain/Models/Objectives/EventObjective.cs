using QuestSystem.Domain.ValueObjects;

namespace QuestSystem.Domain.Models.Objectives
{
    public class EventObjective : BaseObjective<string, string>
    {
        public EventObjective(string description, string metric, string goal) : base(description, metric, goal)
        {
        }

        public override bool CheckObjectiveCompletion(string valueToCheck)
        {
            return valueToCheck.Equals(Goal);
        }
    }
}
