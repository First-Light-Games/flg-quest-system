using System;
using QuestSystem.Domain.Common;
using QuestSystem.Domain.Interfaces;

namespace QuestSystem.Domain.Models.Objectives
{
    public abstract class BaseObjective<TInput, TGoal> : BaseModel, IObjective,  IObjectiveCompletionRule<TInput>
    {
        
        public string Description { get; set; }
    
        public string Metric { get; set; }
        
        public TGoal Goal { get; set; }
        
        private bool _completed;
        
        public bool Completed => _completed;
        

        public BaseObjective(string description, string metric, TGoal goal)
        {
            Description = description;
            Metric = metric;
            Goal = goal;
            _completed = false;
        }

        public abstract bool CheckObjectiveCompletion(TInput valueToCheck);
        
        public bool CheckObjectiveCompletion(object valueToCheck)
        {
            if (valueToCheck is TInput typedInput)
            {
                _completed = CheckObjectiveCompletion(typedInput);
                return _completed;
            }
            throw new ArgumentException($"Invalid type for check the quest completion - Input was: {valueToCheck.GetType().Name}, and the input expected for this quest configuration {typeof(TInput).Name}");
        }
    }
}
