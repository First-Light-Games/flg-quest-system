using System.Collections.Generic;
using QuestSystem.Domain.Common;
using QuestSystem.Domain.Exceptions;

namespace QuestSystem.Domain.ValueObjects
{
    public class PredictValues<TInput> : ValueObject
    {
        public TInput SnapshotValue { get; }

        public TInput NewValue { get; }

        public PredictValues(TInput snapshotValue, TInput newValue)
        {
            if (snapshotValue == null || newValue == null)
            {
                throw new PredictableValuesIsNullException();
            }
            
            SnapshotValue = snapshotValue;
            NewValue = newValue;
        }


        protected override IEnumerable<object> GetEqualityComponents()
        {
            if (SnapshotValue != null)
            {
                yield return SnapshotValue;
            }
        }
    }
}
