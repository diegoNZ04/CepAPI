using Cep.Application.DTOs.Requests;
using Cep.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Cep.API.Controllers;

#pragma warning disable CS1591
[ApiController]
[Route("api/ceps")]
public class CepController : ControllerBase
{
    private readonly ICepService _cepService;
    public CepController(ICepService cepService)
    {
        _cepService = cepService;
    }
    /// <summary>
    ///  Consulta o Cep informado através da requisição e registra os dados.
    /// </summary>
    /// <param name="request"></param>
    /// <returns>Retorna os dados de logradouro, cidade, estao e bairro.</returns>
    /// <remarks>
    /// Sample request:
    /// POST /api/ceps/post-cep
    /// {
    ///    "cep": "11111111"
    /// }
    /// </remarks>
    /// <response code="201">Código de retorno para o registro dos dados.</response>
    [HttpPost("post-cep")]
    public async Task<IActionResult> PostCep([FromBody] CepRequestDto request)
    {
        var result = await _cepService.CreateCepAsync(request.Cep);
        return CreatedAtAction(nameof(PostCep), new { cep = result.Cep }, result);
    }

    /// <summary>
    /// Consulta os dados pelo Cep, caso estes estejam registrados.
    /// </summary>
    /// <param name="cep"></param>
    /// <returns> Retorna os dados pelo Cep informado.</returns>
    /// <response code="200">Código de retorno para uma consulta bem-sucedida.</response>
    /// <response code="404">Código de retorno para uma consulta não encontrada.</response>
    [HttpGet("get-cep/{cep}")]
    public async Task<IActionResult> GetCep(string cep)
    {
        var cepData = await _cepService.GetCepAsync(cep);

        if (cepData == null)
            return NotFound(new { message = "CEP não encontrado." });

        return Ok(cepData);
    }
}
#pragma warning restore CS1591