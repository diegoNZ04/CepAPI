using Cep.Domain.Models;
using Cep.Infra.Data;
using Cep.Infra.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Cep.Infra.Repositories;

public class CepRepository : ICepRepository
{
    private readonly ApplicationDbContext _context;
    public CepRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddCepAsync(CepModel cep)
    {
        await _context.Ceps.AddAsync(cep);
        await _context.SaveChangesAsync();
    }

    public async Task<CepModel?> GetByCepAsync(string cepCode)
    {
        return await _context.Ceps.FirstOrDefaultAsync(c => c.CepCode == cepCode);
    }
}