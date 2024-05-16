using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using QuestSystem.Application.Quests.DTOs;
using QuestSystem.Domain.Services;
using QuestSystem.Domain.ValueObjects;

namespace QuestSystem.Application.Quests.Queries.CheckPredictableQuestCompletion;

public class CheckPredictableQuestCompletionQuery : IRequest<QuestCompletedDTO>
{
    public string metric { get; set; } = null!;

    public object SnapshotValue { get; set; } = null!;

    public object NewValue { get; set; } = null!;
}

public class CheckPredictableQuestCompletionQueryHandler : IRequestHandler<CheckPredictableQuestCompletionQuery, QuestCompletedDTO>
{

    private QuestService _questService;

    public CheckPredictableQuestCompletionQueryHandler(QuestService questService)
    {
        _questService = questService;
    }

    public Task<QuestCompletedDTO> Handle(CheckPredictableQuestCompletionQuery request, CancellationToken cancellationToken)
    {

        var snapshotValueType = request.SnapshotValue.GetType();
        var newValueType = request.NewValue.GetType();

        if (snapshotValueType != newValueType)
        {
            throw new ArgumentException($"Snapshot value and newValue are from different types: {snapshotValueType.Name} - {newValueType.Name}");
        }

        var genericType = typeof(PredictValues<>);
        var constructedType = genericType.MakeGenericType(snapshotValueType);
        var predictableValue = Activator.CreateInstance(constructedType, new object[] { request.SnapshotValue, request.NewValue });

        return Task.FromResult(new QuestCompletedDTO()
        {
            Completed = _questService.CheckQuestCompletion(request.metric, predictableValue!)
        });
    }
    
    
}
