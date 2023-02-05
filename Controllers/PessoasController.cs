using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using WebApi.AWS;
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
        private readonly ITranslateService _translateService;

        public PessoasController(
            IMapper mapper, 
            ICepApiService paymentService, 
            IConselhoApiService conselhoService, 
            ITranslateService translateService)
        {
            _mapper = mapper;
            _paymentService = paymentService;
            _conselhoService = conselhoService;
            _translateService = translateService;
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
        [Route("cep1")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCep1([FromQuery] string cep)
        {
            var endereco = new CepResponse();
            var enderecoTask = _paymentService.GetAddressAsync(cep);
            var conselhoTask = _conselhoService.GetConselhoAsync();

            await Task.WhenAll(enderecoTask, conselhoTask)
                .ContinueWith(task =>
                {
                    if (task.IsFaulted)
                    {                        
                        Console.WriteLine("Erro ao chamar a task: " + task.Exception.InnerException);
                    }
                });

            if (enderecoTask.IsCompletedSuccessfully)
            {
                endereco = enderecoTask.Result;

                string conselhoTraduzido = "";
                if (conselhoTask.IsCompletedSuccessfully)
                {
                    var conselho = conselhoTask.Result;
                    conselhoTraduzido = await _translateService.TranslateAsyncRefactor(conselho.slip.advice, "pt-br");
                }
                endereco.conselho = conselhoTraduzido;
            }

            return Ok(endereco);
        }

        [HttpGet]
        [Route("cep")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCep([FromQuery] string cep)
        {           
            var enderecoTask = _paymentService.GetAddressAsync(cep);
            var conselhoTask = _conselhoService.GetConselhoAsync();

            await Task.WhenAll(enderecoTask, conselhoTask)
                .ContinueWith(task =>{
                    if (task.IsFaulted)Console.WriteLine("Erro ao chamar a task: " + task.Exception.InnerException);
                });

            var endereco = (enderecoTask.IsCompletedSuccessfully) ? enderecoTask.Result : new CepResponse();
            var conselho = (conselhoTask.IsCompletedSuccessfully) ? conselhoTask.Result : new ConselhoResponse();               
                               
            string conselhoTraduzido = await _translateService.TranslateAsyncRefactor(conselho.slip.advice, "pt-br");

            endereco.conselho = conselhoTraduzido;

            return Ok(endereco);            
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
