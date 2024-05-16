using System;

namespace QuestSystem.Domain.Common
{
    public abstract class BaseAuditableModel : BaseModel
    {
        protected BaseAuditableModel(string lastModifiedBy)
        {
            LastModifiedBy = lastModifiedBy;
        }

        public DateTimeOffset Created { get; set; }

        public string? CreatedBy { get; set; }

        public DateTimeOffset LastModified { get; set; }

        public string LastModifiedBy { get; set; }
    }
}
