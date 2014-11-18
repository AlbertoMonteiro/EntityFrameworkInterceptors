using System.Data.Entity.Infrastructure.Interception;

namespace Auditer
{
    public class DeleteCommandHandler : CommandHandler<int>
    {
        const string DELETE_PATTERN = @"DELETE \[dbo\]\.\[(?<table>\w+)\]\r?\n?WHERE \(\[Id\] = (?<id>@\d+)\)";
        public DeleteCommandHandler(DbCommandInterceptionContext<int> context)
            : base(context)
        {
            Kind = AuditEntryKind.Delete;
            Pattern = DELETE_PATTERN;
        }
    }
}