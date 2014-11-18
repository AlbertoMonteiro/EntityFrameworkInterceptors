using System;
using System.Data.Common;
using System.Data.Entity.Infrastructure.Interception;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace EntityFrameworkInterceptors
{
    public class AuditingCommandInterceptor : IDbCommandInterceptor
    {
        public void NonQueryExecuting(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
            Console.WriteLine("{0}\n\t{1}", MethodBase.GetCurrentMethod().Name, command.CommandText);
        }

        public void NonQueryExecuted(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
            Console.WriteLine("{0}\n\t{1}", MethodBase.GetCurrentMethod().Name, command.CommandText);
        }

        public void ReaderExecuting(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
        }

        public void ReaderExecuted(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
            const string PATTERN = @"INSERT\s\[dbo\]\.\[(?<table>\w+)\]\([\[\]\w,\s]+\)\r\nVALUES\s\(((,\s)?@[\d])+\)";
            var regex = new Regex(PATTERN, RegexOptions.None);
            var match = regex.Match(command.CommandText);
            if (match.Success && interceptionContext.OriginalResult.HasRows)
            {
                string values = string.Join(";", command.Parameters.Cast<DbParameter>().Select(x => x.ParameterName + ":" + x.Value));
                interceptionContext.OriginalResult.Read();
                
                var audit = new AuditEntry
                {
                    Created = DateTime.Now,
                    Kind = AuditEntryKind.Insert,
                    NewValue = values,
                    Table = match.Groups["table"].Value,
                    Transaction = command.Transaction.GetHashCode(),
                    EntityId = interceptionContext.OriginalResult.GetInt64(0)
                };
                interceptionContext.OriginalResult.Close();
                command.CommandText = string.Format(@"SELECT IDENT_CURRENT('{0}') as [Id];", audit.Table);
                interceptionContext.Result = command.ExecuteReader();
                Console.WriteLine(audit);
            }
        }

        public void ScalarExecuting(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
        }

        public void ScalarExecuted(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
        }
    }
}