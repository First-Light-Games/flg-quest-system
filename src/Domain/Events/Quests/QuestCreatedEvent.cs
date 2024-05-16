using QuestSystem.Domain.Common;
using QuestSystem.Domain.Models;
using QuestSystem.Domain.Models.Quests;

namespace QuestSystem.Domain.Events.Quests
{
    public class QuestCreatedEvent : BaseEvent
    {
        public QuestCreatedEvent(Quest quest)
        {
            Quest = quest;
        }

        public Quest Quest { get; }
    }
}
