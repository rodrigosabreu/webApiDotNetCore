using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Refit;
using System;
using System.Threading.Tasks;
using WebApi.Dtos;
using WebApi.Entidades;
using WebApi.Refit;

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
        public async Task<IActionResult> GetPessoa([FromQuery] PessoaDto parametros)
        {
            var result = _mapper.Map<Pessoa>(parametros);
            return Ok(result);
        }

        [HttpGet]
        [Route("cep")]
        public async Task<IActionResult> GetCep([FromQuery] string cep)
        {            
            var result = await _paymentService.GetAddressAsync(cep);
            return Ok(result);
        }
    }
}
