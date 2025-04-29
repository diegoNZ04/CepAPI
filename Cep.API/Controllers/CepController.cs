using Cep.Application.DTOs.Requests;
using Cep.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Cep.API.Controllers;

[ApiController]
[Route("api/ceps")]
public class CepController : ControllerBase
{
    private readonly ICepService _cepService;
    public CepController(ICepService cepService)
    {
        _cepService = cepService;
    }
    [HttpPost("post-cep")]
    public async Task<IActionResult> PostCep([FromBody] CepRequestDto request)
    {
        var result = await _cepService.CreateCepAsync(request.Cep);
        return CreatedAtAction(nameof(PostCep), new { cep = result.Cep }, result);
    }

    [HttpGet("get-cep/{cep}")]
    public async Task<IActionResult> GetCep(string cep)
    {
        var cepData = await _cepService.GetCepAsync(cep);

        if (cepData == null)
            return NotFound(new { message = "CEP não encontrado." });

        return Ok(cepData);
    }
}
