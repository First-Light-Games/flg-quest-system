namespace QuestSystem.Domain.Interfaces
{
    public interface IObjectiveCompletionRule<TInput>
    {

        bool CheckObjectiveCompletion(TInput valueToCheck);

    }
}
