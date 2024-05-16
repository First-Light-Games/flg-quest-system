using QuestSystem.Domain.Common;
using QuestSystem.Domain.Events.Quests;
using QuestSystem.Domain.Interfaces;

namespace QuestSystem.Domain.Models.Quests
{
    public class Quest : BaseModel
    {
        
        public string Title { get; set; }
        
        public string Description { get; set; }
        
        public IObjective Objective { get; set; }

        
        public Quest(string title, string description, IObjective objective)
        {
            Title = title;
            Description = description;
            Objective = objective;
        }
        
        public bool Completed()
        {
            if (Objective.Completed)
            {
                AddDomainEvent(new QuestCompletedEvent(this));
            }

            return Objective.Completed;
        }
    }
}
