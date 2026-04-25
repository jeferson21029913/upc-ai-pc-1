using Microsoft.AspNetCore.Mvc;
using UpcAiPc1.Application.Exceptions;
using UpcAiPc1.Application.Services;

namespace UpcAiPc1.Web.Controllers;

[ApiController]
public sealed class JsonDataController(IJsonPayloadService jsonPayloadService) : ControllerBase
{
    [HttpGet("/")]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        try
        {
            var payload = await jsonPayloadService.GetPayloadAsync(cancellationToken);
            return Ok(payload);
        }
        catch (InvalidSourceJsonException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
        catch (FileNotFoundException)
        {
            return NotFound(new { mensaje = "No se encontro el archivo de origen JSON." });
        }
    }
}
