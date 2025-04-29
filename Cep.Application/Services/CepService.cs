using AutoMapper;
using Cep.Application.DTOs.Responses;
using Cep.Application.Exceptions;
using Cep.Application.Services.Interfaces;
using Cep.Domain.Models;
using Cep.Infra.Repositories.Interfaces;

namespace Cep.Application.Services;

public class CepService : ICepService
{
    private readonly ICepRepository _cepRepository;
    private readonly IViaCepService _viaCepService;
    private readonly IMapper _mapper;

    public CepService(ICepRepository cepRepository, IViaCepService viaCepService, IMapper mapper)
    {
        _cepRepository = cepRepository;
        _viaCepService = viaCepService;
        _mapper = mapper;
    }

    public async Task<CepResponseDto> CreateCepAsync(string cep)
    {
        var existingCep = await _cepRepository.GetByCepAsync(cep);

        if (existingCep != null)
            return _mapper.Map<CepResponseDto>(existingCep);

        var viaCepDto = await _viaCepService.FetchCepAsync(cep);

        if (viaCepDto == null)
            throw new NotFoundException("Cep não encontrado no serviço externo.");

        var newCep = _mapper.Map<CepModel>(viaCepDto);

        await _cepRepository.AddCepAsync(newCep);

        return _mapper.Map<CepResponseDto>(newCep);
    }

    public async Task<CepResponseDto> GetCepAsync(string cep)
    {
        var cepModel = await _cepRepository.GetByCepAsync(cep);

        if (cepModel == null)
            throw new NotFoundException("Cep não encontrado.");

        return _mapper.Map<CepResponseDto>(cepModel);
    }
}