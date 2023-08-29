using Application.Common.Interfaces;
using Application.Features.Statistics.Queries.GetTotalCount;
using Application.Features.SystemConfiguration.Queries.GetSMSConfiguration;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RestApi.Controllers;

[Authorize]
public class DashbordController : Controller
{

    private readonly ISender _mediator;
    private readonly ILogger<DashbordController> _logger;

    public DashbordController(ISender mediator, IApplicationDbContext dbcontext, ILogger<DashbordController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }
    [Route("Dashbord/GetTotal")]
    [HttpGet]
    public async Task<IActionResult> GetTotal()
    {
        try
        {
            var result = await _mediator.Send(new GetTotalCountQuery());
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Get Total Request Message :{ex.Message} \n StackTrace :{ex.StackTrace}");
            return BadRequest(ex.StackTrace);
        }

    }
    [Route("Dashbord/GetStatistics")]
    [HttpGet]
    public async Task<IActionResult> GetStatistics()
    {
        var result = await _mediator.Send(new GetStatisticsByWeekOfMonthQuery());
        return Ok(result);
    }
}
