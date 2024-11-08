using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using QuestSystem.Domain.Exceptions;
using QuestSystem.Domain.Models.Quests;

namespace QuestSystem.Domain.Services
{
    public class QuestService
    {

        private ConcurrentDictionary<string, Quest> _questsConfigured = new();

        
        public void ConfigureQuest(Quest quest)
        {
            ValidateQuest(quest);

            if (!_questsConfigured.TryAdd(quest.Title, quest))
            {
                throw new QuestAlreadyActiveForMetricException(quest.Objective.Metric);
            }
        }

        
        public bool CheckQuestCompletion(string questTitle, object playerCurrentValue)
        {

            if (!_questsConfigured.TryGetValue(questTitle, out Quest existingQuest))
            {
                throw new QuestNotFoundWithTitleException(questTitle);
            }
            
            existingQuest.Objective.CheckObjectiveCompletion(playerCurrentValue);

            return existingQuest.Objective.Completed;
        }

        
        public bool CheckPlatformQuestCompletion(PlatformQuest platformQuest, object playerCurrentValue)
        {
            
            platformQuest.Objective.CheckObjectiveCompletion(playerCurrentValue);

            return platformQuest.Objective.Completed;
        }

        

        public List<Quest> ListActiveQuests()
        {
            return _questsConfigured.Values.ToList();
        }

        
        private void ValidateQuest(Quest quest)
        {
            if (quest == null)
            {
                throw new ArgumentNullException(nameof(quest), "To configure a new quest, the quest parameter cannot be null.");
            }

            if (quest.Objective == null)
            {
                throw new ArgumentNullException(nameof(quest.Objective), "A quest must have one Objective linked to it.");
            }

            if (quest.Objective.Metric == null)
            {
                throw new ArgumentNullException(nameof(quest.Objective.Metric), "An Objective must have a valid Metric and metric name!");
            }
        }
    }
}
