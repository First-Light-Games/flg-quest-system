using QuestSystem.Domain.ValueObjects;

namespace QuestSystem.Domain.Models.Objectives
{
    public class AmountPredictableObjective : BaseObjective<PredictValues<int>, int>
    {
        public AmountPredictableObjective(string description, string metric, int goal) : base(description, metric, goal)
        {
        }

        public override bool CheckObjectiveCompletion(PredictValues<int> valueToCheck)
        {
            return (valueToCheck.NewValue - valueToCheck.SnapshotValue) >= Goal;
        }
    }
}
