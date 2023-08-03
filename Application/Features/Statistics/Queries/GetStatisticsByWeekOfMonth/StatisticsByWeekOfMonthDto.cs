namespace SignTEC.Application.Features.SystemConfiguration.Queries.GetSMSConfiguration;

public class StatisticsByWeekOfMonthDto
{
    public SeriesDto Series { get; set; }
    public string[] xAxis { get; set; }
}
