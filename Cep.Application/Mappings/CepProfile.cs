using AutoMapper;
using Cep.Application.DTOs;
using Cep.Application.DTOs.Requests;
using Cep.Application.DTOs.Responses;
using Cep.Domain.Models;

namespace Cep.Application.Mappings;

public class CepProfile : Profile
{
    public CepProfile()
    {
        CreateMap<CepModel, CepResponseDto>()
            .ForMember(dest => dest.Cep, opt => opt.MapFrom(src => src.CepCode));

        CreateMap<ViaCepDto, CepModel>()
            .ForMember(dest => dest.CepCode, opt => opt.MapFrom(src => src.Cep.Replace("-", "")))
            .ForMember(dest => dest.Cidade, opt => opt.MapFrom(src => src.Localidade))
            .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => src.Uf));
    }
}