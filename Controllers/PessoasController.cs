using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Refit;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using WebApi.Dtos;
using WebApi.Entidades;
using WebApi.Refit;
using WebApi.Repositories;
using WebApi.Response;
using WebApi.Services;

namespace WebApi.Controllers
{
    [Route("api")]
    [ApiController]
    public class PessoasController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICepApiService _paymentService;
        private readonly IConselhoApiService _conselhoService;

        public PessoasController(IMapper mapper, ICepApiService paymentService, IConselhoApiService conselhoService)
        {
            _mapper = mapper;
            _paymentService = paymentService;
            _conselhoService = conselhoService;
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
            var endereco = _paymentService.GetAddressAsync(cep);
            var conselho = _conselhoService.GetConselhoAsync();

            await Task.WhenAll(endereco, conselho);

            endereco.Result.afonso_meu_senior = conselho.Result.slip.advice;                      

            return Ok(endereco.Result);

            /*var ceps = new List<string> { "08676250", "08615050", "08673115" };
            var result2 = await Task.WhenAll(ceps.Select(cep => _paymentService.GetAddressAsync(cep)));                       

            // Crie uma lista de tarefas para paralelizar as chamadas
            var tasks = new List<Task<CepResponse>>();

            for (int i = 0; i < 10; i++)
            {
                tasks.Add(_paymentService.GetAddressAsync(cep));
            }

            var result4 = await Task.WhenAll(_conselhoService.GetConselhoAsync());

            // Espere a finalização de todas as tarefas
            var result3 = await Task.WhenAll(tasks);   */


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
        [Route("token")]
        [AllowAnonymous]
        public async Task<IActionResult> GetToken([FromHeader] string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadJwtToken(token);

            var result = new { jsonToken.Header, jsonToken.Payload};

            return Ok(result);
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
