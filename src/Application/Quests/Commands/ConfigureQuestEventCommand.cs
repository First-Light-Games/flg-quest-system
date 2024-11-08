using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using QuestSystem.Application.Quests.DTOs;
using QuestSystem.Domain.Enums;
using QuestSystem.Domain.Events.Quests;
using QuestSystem.Domain.Interfaces;
using QuestSystem.Domain.Models.Objectives;
using QuestSystem.Domain.Models.Quests;
using QuestSystem.Domain.Services;

namespace QuestSystem.Application.Quests.Commands;

public static class ConfigureQuest {

    
    public record Command : IRequest
    {
        public required QuestDTO Quest { get; set; }
    }
    
    
    public class ConfigureQuestCommandValidator : AbstractValidator<ConfigureQuest.Command>
    {
        public ConfigureQuestCommandValidator()
        {
            // RuleFor(v => v.Title)
            //     .MaximumLength(200)
            //     .NotEmpty();
            //
            // RuleFor(v => v.Description)
            //     .MaximumLength(200)
            //     .NotEmpty();
        }
    }
    

    public class Handler : IRequestHandler<ConfigureQuest.Command>
    {
        private readonly QuestService _questService;

        public Handler(QuestService questService)
        {
            _questService = questService;
        }

        public Task Handle(ConfigureQuest.Command request, CancellationToken cancellationToken)
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
                    objectiveData.Goal);
            }
            
            return objective;
        }
    }
}
