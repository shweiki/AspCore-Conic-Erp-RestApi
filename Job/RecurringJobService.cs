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
            string monthlyIntervalCron = Cron.Monthly(1);
            string dailyIntervalCronActivated3AM = Cron.Daily(3);
            string hourIntervalCron = Cron.Hourly(6);
            string minuteIntervalCron = Cron.Minutely();

            if (_recurringJobManager is null)
            {
                return Task.CompletedTask;
            }

            _recurringJobManager.AddOrUpdate<MediatorHelper>("ScanMemberStatueJobCommand", x => x.ScanMemberStatueJobCommand(), dailyIntervalCronActivated3AM);
            _recurringJobManager.AddOrUpdate<MediatorHelper>("RecoveryDataBaseJobCommand", x => x.RecoveryDataBaseJobCommand(), dailyIntervalCronActivated3AM);
            _recurringJobManager.AddOrUpdate<MediatorHelper>("CheckDeviceLogJobCommand", x => x.CheckDeviceLogJobCommand(), dailyIntervalCronActivated3AM);

            _recurringJobManager.AddOrUpdate<MediatorHelper>("FixBase64ToPathWithLoadedJobCommand", x => x.FixBase64ToPathWithLoadedJobCommand(), monthlyIntervalCron);
            _recurringJobManager.AddOrUpdate<MediatorHelper>("FixPhoneNumberJobCommand", x => x.FixPhoneNumberJobCommand(), monthlyIntervalCron);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "An exception occured while creating recurring jobs.");
        }

        return Task.CompletedTask;
    }
}
