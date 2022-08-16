using Refit;
using System.Threading.Tasks;
using WebApi.Response;

namespace WebApi.Refit
{
    public interface ICepApiService
    {
        [Get("/ws/{cep}/json")]
        Task<CepResponse> GetAddressAsync(string cep);
    }
}
