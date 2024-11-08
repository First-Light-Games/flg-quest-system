using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using QuestSystem.Application.Common.Interfaces;
using QuestSystem.Application.Quests.DTOs;
using QuestSystem.Domain.Enums;
using QuestSystem.Domain.Interfaces;
using QuestSystem.Domain.Models.Objectives;
using QuestSystem.Domain.Models.Quests;

namespace QuestSystem.Application.Quests.Commands;

public static class ConfigurePlatformQuest {
    
    public record Command : IRequest<PlatformQuestEventKeyDTO>
    {
        public required QuestDTO Quest { get; set; }
    
        public required DateTime PlatformQuestStartDate { get; set; }
    
        public required DateTime PlatformQuestEndDate { get; set; }
    }

    public class ConfigurePlatformQuestValidator : AbstractValidator<ConfigurePlatformQuest.Command>
    {
        public ConfigurePlatformQuestValidator()
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


    internal sealed class Handler : IRequestHandler<ConfigurePlatformQuest.Command, PlatformQuestEventKeyDTO>
    {
        private readonly ISecureDataService _secureDataService;

        
        public Handler(ISecureDataService secureDataService)
        {
            _secureDataService = secureDataService;
        }

        public Task<PlatformQuestEventKeyDTO> Handle(ConfigurePlatformQuest.Command request, CancellationToken cancellationToken)
        {
            var questEvent = new PlatformQuest(
                request.Quest.Title,
                request.Quest.Description,
                ConfigureQuestObjective(request.Quest),
                request.PlatformQuestStartDate,
                request.PlatformQuestEndDate
            );
        
            var platformQuestEventKey = _secureDataService.Encrypt(questEvent); 
        
        
            return Task.FromResult(new PlatformQuestEventKeyDTO() {PlatformQuestEventKey = platformQuestEventKey});
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
