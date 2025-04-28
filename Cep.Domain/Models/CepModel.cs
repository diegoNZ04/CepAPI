using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cep.Domain.Models;

[Table("ceps_diego")]
public class CepModel
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string CepCode { get; set; }
    public string Logradouro { get; set; }
    public string Bairro { get; set; }
    public string Cidade { get; set; }
    public string Estado { get; set; }
}
