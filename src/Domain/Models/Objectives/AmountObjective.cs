using QuestSystem.Domain.ValueObjects;

namespace QuestSystem.Domain.Models.Objectives
{
    /// <summary>
    /// Represents an objective where completion is determined by reaching or exceeding a specified numerical amount.
    /// </summary>
    public class AmountObjective : BaseObjective<int, int>
    {
        public AmountObjective(string description, string metric, int goal) : base(description, metric, goal)
        {
        }
        
        public override bool CheckObjectiveCompletion(int valueToCheck)
        {
            return valueToCheck >= Goal;
        }
    }
}
