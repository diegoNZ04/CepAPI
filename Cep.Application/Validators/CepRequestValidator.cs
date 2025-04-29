using Cep.Application.DTOs.Requests;
using FluentValidation;

namespace Cep.Application.Validators;

public class CepRequestValidator : AbstractValidator<CepRequestDto>
{
    public CepRequestValidator()
    {
        RuleFor(x => x.Cep)
            .NotEmpty().WithMessage("O Cep é obrigatório.")
            .Length(8).WithMessage("O Cep deve conter 8 dígitos.")
            .Matches(@"^\d{8}$").WithMessage("O CEP deve conter apenas números.");
    }
}