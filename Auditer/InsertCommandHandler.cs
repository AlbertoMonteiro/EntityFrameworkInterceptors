using System.Data.Common;
using System.Data.Entity.Infrastructure.Interception;

namespace Auditer
{
    public class InsertCommandHandler : CommandHandler<DbDataReader>
    {
        const string INSERT_PATTERN = @"INSERT \[dbo\]\.\[(?<table>\w+)\]\(((?<field>\[\w+\]),? ?)+\)\r?\n?VALUES \(.+?\)";

        public InsertCommandHandler(DbCommandInterceptionContext<DbDataReader> context)
            : base(context)
        {
            Kind = AuditEntryKind.Insert;
            Pattern = INSERT_PATTERN;
            BeforeCreatedAudit = () => context.OriginalResult.Read();
            AfterCreatedAudit = (command, table) =>
            {
                context.OriginalResult.Close();
                command.CommandText = string.Format(@"SELECT IDENT_CURRENT('{0}') as [Id];", table);
                context.Result = command.ExecuteReader();
            };
        }
    }
}