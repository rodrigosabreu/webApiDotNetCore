using System;

namespace WebApi.Dtos
{
    public class PessoaDto
    {        
        public string Nome { get; set; }        
        public string Sobrenome { get; set; }
        public DateTime DataNascimento { get; set; }
    }
}
