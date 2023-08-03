using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Statistics.Queries.GetTotalCount;


public class GetTotalCountQuery : IRequest<object>
{

}
public class GetTotalCountQueryHandler : IRequestHandler<GetTotalCountQuery, object>
{
    private readonly IApplicationDbContext _context;

    public GetTotalCountQueryHandler(IApplicationDbContext context)
    {
        _context = context;

    }

    public async Task<object> Handle(GetTotalCountQuery request, CancellationToken cancellationToken)
    {

        DateTime LastWeek = DateTime.Today.AddDays(-6);
        var itemsQuery = _context.SalesInvoice.Where(x => x.Status == 1 && x.FakeDate >= LastWeek).AsQueryable();

        int totalCountBeforeFilter = await itemsQuery.CountAsync();
        var items = await itemsQuery.ToListAsync();
        var SalelastWeek = items.Select(x => new
        {
            Total = x.InventoryMovements.Sum(s => s.SellingPrice * s.Qty) - x.Discount,
            x.FakeDate
        }).GroupBy(a => new { a.FakeDate.DayOfWeek }).Select(g => new
        {
            Key = g.First().FakeDate.ToString("ddd"),
            Value = g.Sum(d => d.Total),
        }).ToList();
        var Data = new
        {
            Items = _context.Item.Count(),
            Purchases = _context.PurchaseInvoice.Count(),
            Sales = new
            {
                Count = totalCountBeforeFilter,
                xAxisdata = SalelastWeek.Select(x => x.Key),
                expectedData = SalelastWeek.Select(l => l.Value),
                actualData = SalelastWeek.Select(l => l.Value)
            },
            Clients = _context.Vendor.Where(x => x.Type == "Customer").Count(),
            Suppliers = _context.Vendor.Where(x => x.Type == "Supplier").Count(),
            Members = _context.Member.Count(),
            MembersActive = _context.Member.Where(x => x.Status >= 0).Count(),
            MsgCredit = 0
        };
        return Data;
    }
}
