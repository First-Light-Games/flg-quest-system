using System.Threading.Tasks;

namespace QuestSystem.Application.Common.Interfaces;

public interface IQueueContext<T>
{
    Task<T> Pop();

    Task Push(T item);
}
