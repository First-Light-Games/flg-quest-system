using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using QuestSystem.Application.Common.Interfaces;
using QuestSystem.Application.Quests.DTOs;
using QuestSystem.Domain.Models.Quests;
using QuestSystem.Domain.Services;

namespace QuestSystem.Application.Quests.Queries.CheckPlatformQuestCompletion;

public class CheckPlatformQuestCompletionQuery : IRequest<QuestCompletedDTO>
{
    public string PlayerIdentifier  { get; set; } = null!;
    
    public string PlatformQuestEventKey  { get; set; } = null!;
}

public class CheckQuestCompletionQueryHandler : IRequestHandler<CheckPlatformQuestCompletionQuery, QuestCompletedDTO>
{
    
    private readonly IMetricProvider _metricProvider;
    private readonly ISecureDataService _secureDataService;
    private readonly QuestService _questService;

    public CheckQuestCompletionQueryHandler(QuestService questService, IMetricProvider metricProvider, ISecureDataService secureDataService)
    {
        _questService = questService;
        _metricProvider = metricProvider;
        _secureDataService = secureDataService;
    }

    public async Task<QuestCompletedDTO> Handle(CheckPlatformQuestCompletionQuery request, CancellationToken cancellationToken)
    {
        var platformQuest = _secureDataService.Decrypt<PlatformQuest>(request.PlatformQuestEventKey);

        if (platformQuest == null)
        {
            throw new UnauthorizedAccessException("Invalid Api-Key");
        }

        var playerMetric = await _metricProvider.GetMetricFromUser(request.PlayerIdentifier, platformQuest.Objective.Metric);
    
        return await Task.FromResult(new QuestCompletedDTO()
        {
            Completed = _questService.CheckPlatformQuestCompletion(platformQuest, playerMetric!.Value ?? throw new InvalidOperationException())
        });
    
    }
}
