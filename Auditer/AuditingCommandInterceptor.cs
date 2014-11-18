using System;
using System.Data.Common;
using System.Data.Entity.Infrastructure.Interception;

namespace Auditer
{
    public class AuditingCommandInterceptor : IDbCommandInterceptor
    {
        private readonly Action<AuditEntry> log;
        private readonly Action<string> vaiQueEhTua;

        public AuditingCommandInterceptor(Action<AuditEntry> log, Action<string> vaiQueEhTua)
        {
            this.log = log;
            this.vaiQueEhTua = vaiQueEhTua;
        }

        #region Uneeded methods
        public void NonQueryExecuted(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
        }

        public void ReaderExecuting(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
        }

        public void ScalarExecuting(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
        }

        public void ScalarExecuted(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
        }
        #endregion

        public void ReaderExecuted(DbCommand command, DbCommandInterceptionContext<DbDataReader> context)
        {
            ICommandHandler insert = new InsertCommandHandler(context);
            var auditEntry = insert.HandleCommand(command);

            if (auditEntry != null)
                log(auditEntry);
        }

        public void NonQueryExecuting(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
            ICommandHandler updateHandler = new UpdateCommandHandler(interceptionContext);
            updateHandler.SetNext(new DeleteCommandHandler(interceptionContext));

            var auditEntry = updateHandler.HandleCommand(command);

            if (auditEntry != null)
                log(auditEntry);
        }
    }
}