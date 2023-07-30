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
            string hourIntervalCron = Cron.Hourly(6);

            if (_recurringJobManager is null)
            {
                return Task.CompletedTask;
            }

            _recurringJobManager.AddOrUpdate<MediatorHelper>("ScanMemberStatueJobCommand", x => x.ScanMemberStatueJobCommand(), dailyIntervalCron);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "An exception occured while creating recurring jobs.");
        }

        return Task.CompletedTask;
    }
}
