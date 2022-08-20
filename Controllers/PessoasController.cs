using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Refit;
using System;
using System.Threading.Tasks;
using WebApi.Dtos;
using WebApi.Entidades;
using WebApi.Refit;
using WebApi.Repositories;
using WebApi.Services;

namespace WebApi.Controllers
{
    [Route("api")]
    [ApiController]
    public class PessoasController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICepApiService _paymentService;

        public PessoasController(IMapper mapper, ICepApiService paymentService)
        {
            _mapper = mapper;
            _paymentService = paymentService;
        }

        [HttpGet]
        [Route("pessoas")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPessoa([FromQuery] PessoaDto parametros)
        {
            var result = _mapper.Map<Pessoa>(parametros);
            return Ok(result);
        }

        [HttpGet]
        [Route("cep")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCep([FromQuery] string cep)
        {            
            var result = await _paymentService.GetAddressAsync(cep);
            return Ok(result);
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<ActionResult<dynamic>> Authenticate([FromBody] User model)
        {
            // Recupera o usuário
            var user = UserRepository.Get(model.Username, model.Password);

            // Verifica se o usuário existe
            if (user == null)
                return NotFound(new { message = "Usuário ou senha inválidos" });

            // Gera o Token
            var token = TokenService.GenerateToken(user);

            // Oculta a senha
            user.Password = "";

            // Retorna os dados
            return  new
            {
                user = user,
                token = token
            };
        }

        [HttpGet]
        [Route("anonymous")]
        [AllowAnonymous]
        public string Anonymous() => "Anônimo";

        [HttpGet]
        [Route("authenticated")]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public string Authenticated() => String.Format("Autenticado - {0}", User.Identity.Name);

        [HttpGet]
        [Route("employee")]
        [Microsoft.AspNetCore.Authorization.Authorize(Roles = "employee,manager")]
        public string Employee() => "Funcionário";

        [HttpGet]
        [Route("manager")]
        [Microsoft.AspNetCore.Authorization.Authorize(Roles = "manager")]
        public string Manager() => "Gerente";
    }
}
