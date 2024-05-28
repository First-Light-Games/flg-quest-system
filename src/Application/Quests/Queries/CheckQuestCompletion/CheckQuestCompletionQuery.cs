using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using QuestSystem.Application.Common.Interfaces;
using QuestSystem.Application.Quests.DTOs;
using QuestSystem.Domain.Services;

namespace QuestSystem.Application.Quests.Queries.CheckQuestCompletion;

public class CheckQuestCompletionQuery : IRequest<QuestCompletedDTO>
{
    public string questName { get; set; } = null!;

    public string playerIdentifier  { get; set; } = null!;
}

public class CheckQuestCompletionQueryHandler : IRequestHandler<CheckQuestCompletionQuery, QuestCompletedDTO>
{

    private readonly QuestService _questService;
    private readonly IMetricProvider _metricProvider;

    public CheckQuestCompletionQueryHandler(QuestService questService, IMetricProvider metricProvider)
    {
        _questService = questService;
        _metricProvider = metricProvider;
    }

    public async Task<QuestCompletedDTO> Handle(CheckQuestCompletionQuery request, CancellationToken cancellationToken)
    {
        var playerMetric = await _metricProvider.GetMetricFromUser(request.playerIdentifier, request.questName);

        return await Task.FromResult(new QuestCompletedDTO()
        {
            Completed = _questService.CheckQuestCompletion(request.questName, playerMetric!.Value ?? throw new InvalidOperationException())
        });
    }
}
