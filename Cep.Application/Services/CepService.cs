using System.Text.Json;
using AutoMapper;
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
    private readonly IMapper _mapper;

    public CepService(ICepRepository cepRepository, HttpClient httpClient, IMapper mapper)
    {
        _cepRepository = cepRepository;
        _httpClient = httpClient;
        _mapper = mapper;
    }

    public async Task<CepResponseDto> CreateCepAsync(string cep)
    {
        var existingCep = await _cepRepository.GetByCepAsync(cep);

        if (existingCep != null)
            return _mapper.Map<CepResponseDto>(existingCep);

        var viaCepDto = await FetchCepFromViaCepAsync(cep);

        if (viaCepDto == null)
            throw new Exception("Cep não encontrado no serviço externo.");

        var newCep = _mapper.Map<CepModel>(viaCepDto);

        await _cepRepository.AddCepAsync(newCep);

        return _mapper.Map<CepResponseDto>(newCep);
    }

    public async Task<CepResponseDto> GetCepAsync(string cep)
    {
        var cepModel = await _cepRepository.GetByCepAsync(cep);

        if (cepModel == null)
            throw new Exception("Cep não encontrado.");

        return _mapper.Map<CepResponseDto>(cepModel);
    }

    private async Task<ViaCepDto> FetchCepFromViaCepAsync(string cep)
    {
        var response = await _httpClient.GetAsync($"https://viacep.com.br/ws/{cep}/json/");

        if (!response.IsSuccessStatusCode)
            throw new Exception("Erro ao consultar a ViaCEP API.");


        var content = await response.Content.ReadAsStringAsync();
        var viaCep = JsonSerializer.Deserialize<ViaCepDto>(content);

        if (viaCep == null || viaCep.Cep == null)
            throw new Exception("Erro ao desserializar a resposta da ViaCEP API.");

        return viaCep;
    }
}