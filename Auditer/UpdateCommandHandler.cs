using System.Data.Entity.Infrastructure.Interception;

namespace Auditer
{
    public class UpdateCommandHandler : CommandHandler<int>
    {
        const string UPDATE_PATTERN = @"UPDATE \[dbo\]\.\[(?<table>\w+)\]\r?\n?SET (\[(?<field>\w+)\] = @(?<fieldParam>\d+),?\s?)+\r?\n?WHERE \(\[Id\] = (?<id>@\d+)\)";
        public UpdateCommandHandler(DbCommandInterceptionContext<int> context)
            : base(context)
        {
            Kind = AuditEntryKind.Update;
            Pattern = UPDATE_PATTERN;
        }
    }
}