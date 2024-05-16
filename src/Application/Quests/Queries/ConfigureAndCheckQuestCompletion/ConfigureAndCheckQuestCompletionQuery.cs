using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using QuestSystem.Application.Common.Interfaces;
using QuestSystem.Application.Quests.DTOs;
using QuestSystem.Domain.Enums;
using QuestSystem.Domain.Events.Quests;
using QuestSystem.Domain.Interfaces;
using QuestSystem.Domain.Models.Objectives;
using QuestSystem.Domain.Models.Quests;
using QuestSystem.Domain.Services;

namespace QuestSystem.Application.Quests.Queries.ConfigureAndCheckQuestCompletion
{
    public class ConfigureAndCheckQuestCompletionQuery : IRequest<QuestCompletedDTO>
    {
        public required QuestDTO Quest { get; set; }
    
        public string PlayerEmail { get; set; } = null!;
    }

    public class CheckQuestCompletionQueryHandler : IRequestHandler<ConfigureAndCheckQuestCompletionQuery, QuestCompletedDTO>
    {

        private readonly QuestService _questService;
        private readonly IMetricProvider _metricProvider;

        public CheckQuestCompletionQueryHandler(QuestService questService, IMetricProvider metricProvider)
        {
            _questService = questService;
            _metricProvider = metricProvider;
        }

        public async Task<QuestCompletedDTO> Handle(ConfigureAndCheckQuestCompletionQuery request, CancellationToken cancellationToken)
        {
            var quest = new Quest(
                request.Quest.Title,
                request.Quest.Description,
                ConfigureQuestObjective(request.Quest)
            );

            quest.AddDomainEvent(new QuestCreatedEvent(quest));

            _questService.ConfigureQuest(quest);
        
            var playerMetricData = await _metricProvider.GetMetricFromUser(request.PlayerEmail, quest.Objective.Metric);

            if (playerMetricData?.Value == null)
            {
                throw new NullReferenceException("Couldn't find Metric");
            }

            return await Task.FromResult(new QuestCompletedDTO()
            {
                Completed = _questService.CheckQuestCompletion(quest.Objective.Metric, playerMetricData.Value)
            });
        }
    
    
    
        private IObjective ConfigureQuestObjective(QuestDTO requestQuest)
        {
            var objectiveData = requestQuest.Objective;
            var objectiveMetric = requestQuest.Objective.Metric;

            IObjective objective = null!;
        
            if (requestQuest.QuestType == QuestType.ReachAmount)
            {
                objective = new AmountObjective(
                    objectiveData.Description,
                    objectiveMetric,
                    (int)(objectiveData.Goal ?? 0));
            }

            if (requestQuest.QuestType == QuestType.TriggerEvent)
            {
                objective = new EventObjective(
                    objectiveData.Description,
                    objectiveMetric,
                    objectiveData.Goal as string ?? string.Empty);
            }
        
            if (requestQuest.QuestType == QuestType.AmountPredictable)
            {
                objective = new AmountPredictableObjective(
                    objectiveData.Description,
                    objectiveMetric,
                    (int)(objectiveData.Goal ?? 0));
            }

            return objective;
        }
    }
}
