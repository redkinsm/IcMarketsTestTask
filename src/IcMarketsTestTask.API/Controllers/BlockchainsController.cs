using IcMarketsTestTask.API.ApiModels.Blockchains;
using IcMarketsTestTask.API.Application.Commands;
using IcMarketsTestTask.API.Application.Queries.Blockchains;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace IcMarketsTestTask.API.Controllers;

[ApiController]
[Route("api/blockchains/")]
public class BlockchainsController : ControllerBase
{
    private readonly ISender _sender;

    public BlockchainsController(ISender sender)
    {
        _sender = sender;
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(GetBlockchainsQueryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetBlockchains([FromQuery] GetBlockchainsApiModel request)
    {
        var query = new GetBlockchainsQuery(request.Symbol);
        var result = await _sender.Send(query, HttpContext.RequestAborted);
        return Ok(result);
    }
    
    [HttpPost("sync")]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SyncBlockchain([FromQuery] GetBlockchainsApiModel request)
    {
        var query = new SyncBlockchainCommand(request.Symbol);
        await _sender.Send(query, HttpContext.RequestAborted);
        return Ok();
    }
}