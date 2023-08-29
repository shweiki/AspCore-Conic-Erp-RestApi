using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace RestApi.Controllers;

[Authorize]
public class ChequeController : Controller
{
    private readonly IApplicationDbContext DB;
    public ChequeController(IApplicationDbContext dbcontext)
    {
        DB = dbcontext;
    }
    [Route("Cheques/GetCheques")]
    [HttpGet]
    public IActionResult GetCheque()
    {
        var Cheques = DB.Cheque.Select(x => new
        {
            x.Id,
            x.BankAddress,
            x.BankName,
            x.ChequeAmount,
            x.FakeDate,
            x.Payee,
            x.PaymentType,
            x.Status,
            x.ChequeNumber,
            x.Currency,
            x.Description,
            x.VendorId,
            x.Vendor.Name,
        }).ToList();



        return Ok(Cheques);
    }

    [Route("Cheques/Create")]
    [HttpPost]
    public IActionResult Create(Cheque collection)
    {
        if (ModelState.IsValid)
        {
            try
            {
                collection.Status = 0;
                DB.Cheque.Add(collection);
                DB.SaveChanges();

            }
            catch
            {
                //Console.WriteLine(collection);
                return Ok(false);
            }
        }
        return Ok(false);
    }


}