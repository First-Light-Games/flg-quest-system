using System;

namespace QuestSystem.Domain.Exceptions
{
    public class QuestNotFoundWithTitleException : Exception
    {
        public QuestNotFoundWithTitleException(string s)
            : base($"Quest not found for Title {s}")
        {
        }
    }
}
