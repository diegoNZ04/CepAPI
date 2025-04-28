using Cep.Application.DTOs.Responses;

namespace Cep.Application.Services.Interfaces;

public interface ICepService
{
    Task<CepResponseDto> CreateCepAsync(string cep);
    Task<CepResponseDto> GetCepAsync(string cep);
}