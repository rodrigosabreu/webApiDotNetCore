using FluentValidation;
using System;
using WebApi.Dtos;

namespace WebApi.Validation
{
    public class PessoaDtoValidator : AbstractValidator<PessoaDto> 
    {
        public PessoaDtoValidator()
        {
            RuleFor(x => x.Nome)
                .NotEmpty()
                    .WithMessage("Nome não deve ser nulo ou vazio");
                
            RuleFor(x => x.Sobrenome)
                .NotEmpty()
                    .WithMessage("Sobrenome não deve ser nulo ou vazio");

            RuleFor(x => x.DataNascimento)
                .NotEmpty()
                    .WithMessage("Data de Nascimento não deve ser nulo ou vazio")
                .LessThan(DateTime.UtcNow)
                    .WithMessage("Data de Nascimento deve ser menor a data atual");
        }
    }
}
