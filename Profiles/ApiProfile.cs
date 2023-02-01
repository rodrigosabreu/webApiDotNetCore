using AutoMapper;
using System.IdentityModel.Tokens.Jwt;
using WebApi.Dtos;
using WebApi.Entidades;
using WebApi.Helpers;

namespace WebApi.Profiles
{
    public class ApiProfile : Profile
    {
        public ApiProfile()
        {
            CreateMap<PessoaDto, Pessoa>()
                .ForMember(
                    dest => dest.NomeCompleto,
                    opt => opt.MapFrom(src => $"{src.Nome} {src.Sobrenome}")
                )
                 .ForMember(
                    dest => dest.Idade,
                    opt => opt.MapFrom(src => src.DataNascimento.ObterIdadeAtual())
                );

            CreateMap<JwtSecurityToken, Jwt>();
        }        
    }
}
