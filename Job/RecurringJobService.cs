using Hangfire;

namespace Jobs;

public class RecurringJobService : BackgroundService
{
    private readonly IRecurringJobManager? _recurringJobManager;
    private readonly Serilog.ILogger _logger;

    public RecurringJobService(IRecurringJobManager? recurringJobManager, Serilog.ILogger logger)
    {
        if (recurringJobManager is null)
        {
            logger.Error("recurringJobManager is null");
        }

        _recurringJobManager = recurringJobManager;
        _logger = logger;
    }

    protected override Task ExecuteAsync(CancellationToken cancellationToken)
    {
        try
        {
            string dailyIntervalCron = Cron.Daily(8);
            string dailyIntervalCronActivated3AM = Cron.Daily(3);
            string hourIntervalCron = Cron.Hourly(6);
            string minuteIntervalCron = Cron.Minutely();
            string neverJustAdd = Cron.Never();

            if (_recurringJobManager is null)
            {
                return Task.CompletedTask;
            }

            _recurringJobManager.AddOrUpdate<MediatorHelper>("ScanMemberStatueJobCommand", x => x.ScanMemberStatueJobCommand(), dailyIntervalCronActivated3AM);
            _recurringJobManager.AddOrUpdate<MediatorHelper>("BackupJobCommand", x => x.BackupJobCommand(), dailyIntervalCronActivated3AM);
            _recurringJobManager.AddOrUpdate<MediatorHelper>("CheckDeviceLogJobCommand", x => x.CheckDeviceLogJobCommand(), dailyIntervalCronActivated3AM);

            _recurringJobManager.AddOrUpdate<MediatorHelper>("FixBase64ToPathWithLoadedJobCommand", x => x.FixBase64ToPathWithLoadedJobCommand(), neverJustAdd);
            _recurringJobManager.AddOrUpdate<MediatorHelper>("GetMemberLogFromZktDataBaseJobCommand", x => x.GetMemberLogFromZktDataBaseJobCommand(), minuteIntervalCron);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "An exception occured while creating recurring jobs.");
        }

        return Task.CompletedTask;
    }
}
