using Cep.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Cep.Infra.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    { }

    public DbSet<CepModel> Ceps { get; set; } = null!;
}