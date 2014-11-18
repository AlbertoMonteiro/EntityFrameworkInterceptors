using System.Data.Common;

namespace Auditer
{
    public interface ICommandHandler
    {
        void SetNext(ICommandHandler nextCommandHandler);
        AuditEntry HandleCommand(DbCommand command);
    }
}