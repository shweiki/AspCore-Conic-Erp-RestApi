
using Application.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RestApi.Helper;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace RestApi.Controllers;

[Authorize]
public class DashbordController : Controller
{

    private readonly IApplicationDbContext DB;
    public IConfiguration _configuration { get; }

    public DashbordController(IApplicationDbContext dbcontext, IConfiguration configuration)
    {
        DB = dbcontext;
        _configuration = configuration;
    }
    [Route("Dashbord/GetTotal")]
    [HttpGet]
    public async Task<IActionResult> GetTotal()
    {

        DateTime LastWeek = DateTime.Today.AddDays(-6);
        var SalelastWeekQuery = await DB.SalesInvoice.Include(x => x.InventoryMovements).Where(x => x.Status == 1 && x.FakeDate >= LastWeek).ToListAsync();
        var SalelastWeek = SalelastWeekQuery.Select(x => new
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
            Items = DB.Item.Count(),
            Purchases = DB.PurchaseInvoice.Count(),
            Sales = new { Count = DB.SalesInvoice.Count(), xAxisdata = SalelastWeek.Select(x => x.Key), expectedData = SalelastWeek.Select(l => l.Value), actualData = SalelastWeek.Select(l => l.Value) },
            Clients = DB.Vendor.Where(x => x.Type == "Customer").Count(),
            Suppliers = DB.Vendor.Where(x => x.Type == "Supplier").Count(),
            Members = DB.Member.Count(),
            MembersActive = DB.Member.Where(x => x.Status >= 0).Count(),
            MsgCredit = 0
        };
        return Ok(Data);

    }
    [HttpGet]
    public async Task<IActionResult> GetStatistics(string By = "MonthOfYear") //MonthOfYear , WeekOfMonth
    {

        var InComeOutCome = await DB.EntryMovement.Where(x => x.Account.Type == "InCome" || x.Account.Type == "OutCome")
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


        var Series = new
        {
            OutCome = InComeOutCome.Select(l => l.Credit),
            InCome = InComeOutCome.Select(l => l.Debit),
            Profit = InComeOutCome.Select(l => l.Debit - l.Credit)
        };

        return Ok(new { InComeOutCome, xAxis = InComeOutCome.Select(x => x.Key), Series });

    }
}
