using System.Threading;
using System.Threading.Tasks;
using MediatR;
using QuestSystem.Application.Quests.DTOs;
using QuestSystem.Domain.Enums;
using QuestSystem.Domain.Events.Quests;
using QuestSystem.Domain.Interfaces;
using QuestSystem.Domain.Models.Objectives;
using QuestSystem.Domain.Models.Quests;
using QuestSystem.Domain.Services;
using QuestSystem.Domain.ValueObjects;

namespace QuestSystem.Application.Quests.Commands.ConfigureQuest;

public record ConfigureQuestCommand : IRequest
{
    public required QuestDTO Quest { get; set; }
    
}



public class ConfigureQuestCommandHandler : IRequestHandler<ConfigureQuestCommand>
{
    private readonly QuestService _questService;

    public ConfigureQuestCommandHandler(QuestService questService)
    {
        _questService = questService;
    }

    public Task Handle(ConfigureQuestCommand request, CancellationToken cancellationToken)
    {
        var quest = new Quest(
            request.Quest.Title,
            request.Quest.Description,
            ConfigureQuestObjective(request.Quest)
        );

        quest.AddDomainEvent(new QuestCreatedEvent(quest));

        _questService.ConfigureQuest(quest);
        
        return Task.CompletedTask;
    }

    //Nasty Code for Test Purpose
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
