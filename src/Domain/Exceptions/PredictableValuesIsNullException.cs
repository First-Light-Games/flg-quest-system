using System;
using QuestSystem.Domain.ValueObjects;

namespace QuestSystem.Domain.Exceptions
{
    public class PredictableValuesIsNullException : Exception
    {
        public PredictableValuesIsNullException()
            : base("Either 'PredictableValue Snapshot' or 'NewValue' must not be null.")
        {
        }
    }
}
