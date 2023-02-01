using Refit;
using System.Threading.Tasks;
using WebApi.Response;

namespace WebApi.Refit
{
    public interface IConselhoApiService
    {
        [Get("/advice")]
        Task<ConselhoResponse> GetConselhoAsync();
    }
}
