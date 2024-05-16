using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using QuestSystem.Domain.Events.Quests;

namespace QuestSystem.Application.Quests.EventHandlers;

public class QuestCompletedEventHandler : INotificationHandler<QuestCompletedEvent>
{
    private readonly ILogger<QuestCompletedEventHandler> _logger;

    public QuestCompletedEventHandler(ILogger<QuestCompletedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(QuestCompletedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("QuestSystem Domain Event: {DomainEvent}", notification.GetType().Name);

        return Task.CompletedTask;
    }
}
