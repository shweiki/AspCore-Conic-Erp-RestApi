using Application.Common.Helper;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace SignTEC.Application.Features.SystemConfiguration.Queries.GetSMSConfiguration;

public class GetStatisticsByWeekOfMonthQuery : IRequest<StatisticsByWeekOfMonthDto>
{

}
public class GetStatisticsByWeekOfMonthQueryHandler : IRequestHandler<GetStatisticsByWeekOfMonthQuery, StatisticsByWeekOfMonthDto>
{
    private readonly IApplicationDbContext _context;

    public GetStatisticsByWeekOfMonthQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<StatisticsByWeekOfMonthDto> Handle(GetStatisticsByWeekOfMonthQuery request, CancellationToken cancellationToken)
    {
        var InComeOutCome = await _context.EntryMovement.Where(x => x.Account.Type == "InCome" || x.Account.Type == "OutCome")
                .Select(x => new
                {
                    Key = "",
                    x.Entry.FakeDate,
                    x.Credit,
                    x.Debit,
                    x.Account.Type,
                    x.Account.Name
                }).GroupBy(a => new { a.FakeDate.Month, a.FakeDate.Year }).Select(g => new
                {
                    Key = g.First().FakeDate.ToString("MM") + "-" + g.First().FakeDate.ToString("yyyy"),
                    g.First().Type,
                    g.First().Name,
                    Credit = Utility.toFixed(g.Sum(d => d.Credit), 2),
                    Debit = Utility.toFixed(g.Sum(d => d.Debit), 2),
                }).ToListAsync();


        //   InComeOutCome.GroupBy(a => new { a.FakeDate.DayOfWeek, a.FakeDate.Month }).Select(g => new

        //    Key = g.First().FakeDate.ToString("dddd") + "-" + g.First().FakeDate.ToString("MM"),


        var Series = new SeriesDto
        {
            OutCome = InComeOutCome.Select(l => l.Credit).ToArray(),
            InCome = InComeOutCome.Select(l => l.Debit).ToArray(),
            Profit = InComeOutCome.Select(l => l.Debit - l.Credit).ToArray()
        };

        return new StatisticsByWeekOfMonthDto { xAxis = InComeOutCome.Select(x => x.Key).ToArray(), Series = Series };
    }
}
