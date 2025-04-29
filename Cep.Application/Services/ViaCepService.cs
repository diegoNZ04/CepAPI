
using System.Text.Json;
using Cep.Application.DTOs;
using Cep.Application.Exceptions;
using Cep.Application.Services.Interfaces;

namespace Cep.Application.Services;

public class ViaCepService : IViaCepService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;

    public ViaCepService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    public async Task<ViaCepDto> FetchCepAsync(string cep)
    {
        var response = await _httpClient.GetAsync($"https://viacep.com.br/ws/{cep}/json/");

        if (!response.IsSuccessStatusCode)
            throw new ExternalServiceException("Erro ao consultar a API ViaCEP.");

        var content = await response.Content.ReadAsStringAsync();
        var viaCep = JsonSerializer.Deserialize<ViaCepDto>(content, _jsonOptions);

        if (viaCep == null || string.IsNullOrWhiteSpace(viaCep.Cep))
            throw new ExternalServiceException("Resposta inválida da API ViaCEP.");

        return viaCep;
    }
}
