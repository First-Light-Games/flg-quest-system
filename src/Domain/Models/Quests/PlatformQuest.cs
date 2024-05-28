using System;
using QuestSystem.Domain.Interfaces;

namespace QuestSystem.Domain.Models.Quests
{
    public class PlatformQuest : Quest
    {
        public PlatformQuest(string title, string description, IObjective objective, DateTime eventStartDate, DateTime eventEndDate) : base(title, description, objective)
        {
            EventStartDate = eventStartDate;
            EventEndDate = eventEndDate;
        }

        public DateTime EventStartDate { get; set; }
        public DateTime EventEndDate { get; set; }
    }
}
