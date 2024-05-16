using System;
using QuestSystem.Domain.ValueObjects;

namespace QuestSystem.Domain.Exceptions
{
    public class QuestAlreadyActiveForMetricException : Exception
    {
        public QuestAlreadyActiveForMetricException(string metric)
            : base($"Quest already active for metric: {metric}")
        {
        }
    }
}
