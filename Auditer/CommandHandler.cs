using System;
using System.Data.Common;
using System.Data.Entity.Infrastructure.Interception;
using System.Linq;
using System.Text.RegularExpressions;

namespace Auditer
{
    public abstract class CommandHandler<T> : ICommandHandler
    {
        const string FORMAT = "Field:{0} - Value: {1}";

        protected readonly DbCommandInterceptionContext<T> context;
        protected ICommandHandler NextHandler;
        protected AuditEntryKind Kind;
        protected string Pattern;
        protected Action BeforeCreatedAudit;
        protected Action<DbCommand, string> AfterCreatedAudit;

        protected CommandHandler(DbCommandInterceptionContext<T> context)
        {
            this.context = context;
        }

        public void SetNext(ICommandHandler nextCommandHandler)
        {
            NextHandler = nextCommandHandler;
        }

        public virtual AuditEntry HandleCommand(DbCommand command)
        {
            var match = Regex.Match(command.CommandText, Pattern);
            if (match.Success && context.Exception == null)
            {
                if (BeforeCreatedAudit != null) BeforeCreatedAudit();
                var auditEntry = CreateAuditEntry(command, Kind, match.Groups, Convert.ToInt64(command.Parameters[match.Groups["id"].Value].Value));
                if (AfterCreatedAudit != null) AfterCreatedAudit(command, auditEntry.Table);
                return auditEntry;
            }
            return NextHandler != null ? NextHandler.HandleCommand(command) : null;
        }

        protected static AuditEntry CreateAuditEntry(DbCommand command, AuditEntryKind kind, GroupCollection groups, long entityId)
        {
            var fields = groups["field"].Captures.Cast<Capture>().Select(x => x.Value);
            var values = fields.Zip(command.Parameters.Cast<DbParameter>(), (t, p) => string.Format(FORMAT, t, p.Value)).ToList();

            var audit = new AuditEntry(kind)
            {
                NewValue = string.Join(";", values),
                Table = groups["table"].Value,
                Transaction = command.Transaction.GetHashCode(),
                EntityId = entityId
            };
            return audit;
        }
    }
}