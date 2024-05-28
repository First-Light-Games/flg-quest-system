using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using QuestSystem.Domain.Models.Quests;
using QuestSystem.Domain.Services;

namespace QuestSystem.Application.Quests.Queries.GetActiveQuests;

public record GetActiveQuestsQuery : IRequest<List<Quest>>
{

}

public class GetActiveQuestsQueryHandler(QuestService questService) : IRequestHandler<GetActiveQuestsQuery, List<Quest>>
{
    private readonly QuestService _questService = questService;

    public Task<List<Quest>> Handle(GetActiveQuestsQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(_questService.ListActiveQuests());
    }
}
