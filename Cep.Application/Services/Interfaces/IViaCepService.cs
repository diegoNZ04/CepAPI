using Cep.Application.DTOs;

namespace Cep.Application.Services.Interfaces;

public interface IViaCepService
{
    Task<ViaCepDto> FetchCepAsync(string cep);
}
