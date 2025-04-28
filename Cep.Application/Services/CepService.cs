using System.Text.Json;
using Cep.Application.DTOs;
using Cep.Application.DTOs.Responses;
using Cep.Application.Services.Interfaces;
using Cep.Domain.Models;
using Cep.Infra.Repositories.Interfaces;

namespace Cep.Application.Services;

public class CepService : ICepService
{
    private readonly ICepRepository _cepRepository;
    private readonly HttpClient _httpClient;

    public CepService(ICepRepository cepRepository, HttpClient httpClient)
    {
        _cepRepository = cepRepository;
        _httpClient = httpClient;
    }

    public async Task<CepResponseDto> CreateCepAsync(string cep)
    {
        var response = await _httpClient.GetAsync($"https://viacep.com.br/ws/{cep}/json/");

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Erro ao consultar a ViaCEP API.");
        }

        var content = await response.Content.ReadAsStringAsync();
        var viaCep = JsonSerializer.Deserialize<ViaCepDto>(content);

        if (viaCep == null || viaCep.Cep == null)
        {
            throw new Exception("Erro ao desserializar a resposta da ViaCEP API.");
        }

        var newCep = new CepModel
        {
            CepCode = viaCep.Cep.Replace("-", ""),
            Logradouro = viaCep.Logradouro,
            Bairro = viaCep.Bairro,
            Cidade = viaCep.Localidade,
            Estado = viaCep.Uf
        };

        await _cepRepository.AddCepAsync(newCep);

        return new CepResponseDto
        {
            Cep = newCep.CepCode,
            Logradouro = newCep.Logradouro,
            Bairro = newCep.Bairro,
            Cidade = newCep.Cidade,
            Estado = newCep.Estado
        };
    }

    public async Task<CepResponseDto> GetCepAsync(string cep)
    {
        var cepEntity = await _cepRepository.GetByCepAsync(cep);

        if (cepEntity == null)
            return null;

        return new CepResponseDto
        {
            Cep = cepEntity.CepCode,
            Logradouro = cepEntity.Logradouro,
            Bairro = cepEntity.Bairro,
            Cidade = cepEntity.Cidade,
            Estado = cepEntity.Estado
        };
    }
}