namespace DqtNotifications.ReportingDbListener.Sql;

public interface IDbBuilder
{
    Task<ApplyEntityDeltaResult> ApplyEntityDelta(EntityDelta entity, CancellationToken cancellationToken);
}
