using QuestSystem.Domain.Common;
using QuestSystem.Domain.Interfaces;

namespace QuestSystem.Domain.Events.Objectives
{
    public class ObjectiveCompletedEvent : BaseEvent
    {
        
        public IObjective Objective { get; }
     
        public ObjectiveCompletedEvent(IObjective objective)
        {
            Objective = objective;
        }

    }
}
