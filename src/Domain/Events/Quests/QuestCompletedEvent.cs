using QuestSystem.Domain.Common;
using QuestSystem.Domain.Models;
using QuestSystem.Domain.Models.Quests;

namespace QuestSystem.Domain.Events.Quests
{
    public class QuestCompletedEvent : BaseEvent
    {
        public QuestCompletedEvent(Quest quest)
        {
            Quest = quest;
        }

        public Quest Quest { get; }
    }
}
