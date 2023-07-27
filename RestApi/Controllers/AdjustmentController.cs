
using Domain.Entities; using Application.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace RestApi.Controllers;

[Authorize]
public class AdjustmentController : Controller
{
    private readonly IApplicationDbContext DB;
    public AdjustmentController(IApplicationDbContext dbcontext)
    {
        DB = dbcontext;
    }
    [Route("Adjustment/Create")]
    [HttpPost]

    public IActionResult Create(Adjustment collection)
    {
        if (ModelState.IsValid)
        {
            try
            {

                DB.Adjustment.Add(collection);
                DB.SaveChanges();
                return Ok(true);
            }
            catch
            {
                //Console.WriteLine(collection);
                return Ok(false);
            }
        }
        return Ok(false);

    }

    [Route("Adjustment/GetAdjustments")]
    [HttpGet]
    public IActionResult GetAdjustments()
    {
        try
        {
            var Adjustments = DB.Adjustment
                .Select(x => new { x.Id, x.Name, x.AdjustmentAmount, x.Type, x.IsStaticAdjustment }).ToList();

            return Ok(Adjustments);
        }
        catch
        {
            //Console.WriteLine(collection);
            return Ok(false);
        }

    }
    [Route("Adjustment/GetRAdjustments")]
    [HttpGet]
    public IActionResult GetRAdjustments()
    {
        try
        {
            var Adjustments = DB.Adjustment
                .Select(x => new { x.Id, x.Name, x.AdjustmentAmount, x.Type, x.IsStaticAdjustment }).ToList();

            return Ok(Adjustments);
        }
        catch
        {
            //Console.WriteLine(collection);
            return Ok(false);
        }
    }
    [Route("Adjustment/GetPAdjustments")]
    [HttpGet]
    public IActionResult GetPAdjustments()
    {
        try
        {
            var Adjustments = DB.Adjustment.Where(x => x.IsStaticAdjustment == true)
                .Select(x => new { x.Id, x.Name, x.AdjustmentAmount, x.Type, x.IsStaticAdjustment }).ToList();

            return Ok(Adjustments);
        }

        catch
        {
            //Console.WriteLine(collection);
            return Ok(false);
        }
    }

    [Route("Adjustment/GetAdjustmentLabel")]
    [HttpGet]
    public IActionResult GetAdjustmentLabel()
    {
        try
        {
            var Adjustments = DB.Adjustment.Select(x => new
            {
                value = x.Id,
                label = x.Name,
                amount = x.AdjustmentAmount,
                type = x.Type,
            }).ToList();
            return Ok(Adjustments);
        }
        catch
        {
            //Console.WriteLine(collection);
            return Ok(false);
        }
    }

}
