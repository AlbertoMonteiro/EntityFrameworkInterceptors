using System;

namespace EntityFrameworkInterceptors
{
    public class AuditEntry
    {
        public long EntityId { get; set; }

        public DateTime Created { get; set; }

        public string Table { get; set; }

        public int Transaction { get; set; }

        public string NewValue { get; set; }

        public AuditEntryKind Kind { get; set; }

        public override string ToString()
        {
            return string.Format("Kind: {0} - Created: {1} - On table: {2} - In Transaction: {3} - EntityId: {4}", Kind, Created, Table, Transaction, EntityId);
        }
    }
}