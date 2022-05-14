using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Entities; 
using Microsoft.AspNetCore.Mvc;

namespace AspCore_Conic_Erp_RestApi.Controllers
{
    [Authorize]
    public class DashbordController : Controller
    {

        private readonly ConicErpContext DB;
        public   DashbordController(ConicErpContext dbcontext)
        {
            DB = dbcontext;
        }        [Route("Dashbord/GetTotal")]
        [HttpGet]
        public IActionResult GetTotal()
        {
            string MsgCredit = "0";
            WebRequest request = WebRequest.Create(
              "http://josmsservice.com/smsonline/GetBalance.cfm?AccName=highfit&AccPass=D7!cT5!SgU0");
            if (request.ContentLength > 0) { 
                request.Credentials = CredentialCache.DefaultCredentials;
                WebResponse response = request.GetResponse();
                string responseFromServer;
                using Stream dataStream = response.GetResponseStream();
                StreamReader reader = new(dataStream);
                responseFromServer = reader.ReadToEnd();
                MsgCredit = responseFromServer;
                response.Close();
            }
            DateTime LastWeek = DateTime.Today.AddDays(- 6);
            var SalelastWeek = DB.SalesInvoices.Where(x => x.Status == 1 && x.FakeDate >= LastWeek )
                .AsEnumerable().Select(x => new {
                    Total = DB.InventoryMovements.Where(ms=> ms.SalesInvoiceId == x.Id).Sum(s => s.SellingPrice * s.Qty) - x.Discount,
                    x.FakeDate
                }).GroupBy(a => new { a.FakeDate.DayOfWeek }).Select(g => new {
                    Key = g.First().FakeDate.ToString("ddd"),
                    Value = g.Sum(d => d.Total),
                }).ToList();
            var Data = new
                {
                    Items = DB.Items.Count(),
                    Purchases = DB.PurchaseInvoices.Count(),
                    Sales = new { Count = DB.SalesInvoices.Count(), xAxisdata = SalelastWeek.Select(x=>x.Key), expectedData = SalelastWeek.Select(l => l.Value), actualData = SalelastWeek.Select(l=>l.Value) },
                    Clients = DB.Vendors.Where(x => x.Type == "Customer").Count(),
                    Suppliers = DB.Vendors.Where(x => x.Type == "Supplier").Count(),
                    Members = DB.Members.Count(),
                    MembersActive = DB.Members.Where(x => x.Status >= 0).Count(),
                    MsgCredit
                };
                return Ok(Data);
        }
        [HttpGet]
        public IActionResult GetStatistics()
        {
            var InComeOutCome = DB.EntryMovements.Where(x => x.Account.Type == "InCome" || x.Account.Type == "OutCome")
                 .Select(x => new {
                      x.Entry.FakeDate,
                      x.Credit,
                      x.Debit,
                      x.Account.Type,
                      x.Account.Name
                 }).GroupBy(a => new { a.FakeDate.Month, a.FakeDate.Year }).Select(g => new
                 {
                      Key =  g.First().FakeDate.ToString("MM") + "-" + g.First().FakeDate.ToString("yyyy"),
                     g.First().Type,
                      Name = g.First().Name,
                      Credit = g.Sum(d => d.Credit),
                      Debit = g.Sum(d => d.Debit),
                  }).ToList();
            var Series = new
            {
                InCome = InComeOutCome.Select(l => l.Debit),
                OutCome = InComeOutCome.Select(l => l.Credit),
                Profit = InComeOutCome.Select(l => l.Debit - l.Credit)
            };

            return Ok(new { InComeOutCome,  xAxis = InComeOutCome.Select(x => x.Key) , Series });

        }
    }
}
