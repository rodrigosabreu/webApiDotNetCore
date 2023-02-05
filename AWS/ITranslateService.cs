using System.Threading.Tasks;

namespace WebApi.AWS
{
    public interface ITranslateService
    {
        Task<string> TranslateAsync(string text, string targetLanguageCode);
        Task<string> TranslateAsyncRefactor(string text, string targetLanguageCode);
    }
}
