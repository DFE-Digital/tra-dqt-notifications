namespace DqtNotifications.ReportingDbListener.Sql;

public enum ApplyEntityDeltaResult
{
    Success = 0,
    StaleDataError = 1,
    RowDoesNotExist = 2
}
