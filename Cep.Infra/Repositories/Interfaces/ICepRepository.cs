using Cep.Domain.Models;

namespace Cep.Infra.Repositories.Interfaces;

public interface ICepRepository
{
    Task<CepModel?> GetByCepAsync(string cepCode);
    Task AddCepAsync(CepModel cep);
}