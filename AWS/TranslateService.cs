using Amazon;
using Amazon.Translate;
using Amazon.Translate.Model;
using System;
using System.Threading.Tasks;

namespace WebApi.AWS
{
    public class TranslateService : ITranslateService
    {
        private static readonly RegionEndpoint awsRegion = RegionEndpoint.EUNorth1;
        private const string sourceLanguageCode = "en";

        public async Task<string> TranslateAsync(string text, string targetLanguageCode)
        {            
            AmazonTranslateClient translateClient = new AmazonTranslateClient(Constants.UID, Constants.Secret, awsRegion);
            
            var translateRequest = new TranslateTextRequest
            {
                Text = text,
                SourceLanguageCode = sourceLanguageCode,
                TargetLanguageCode = targetLanguageCode
            };
            
            var translateResponse = await translateClient.TranslateTextAsync(translateRequest);
            
            return translateResponse.TranslatedText;
        }

        public async Task<string> TranslateAsyncRefactor(string text, string targetLanguageCode)
        {
            try
            {
                AmazonTranslateClient translateClient = new AmazonTranslateClient(Constants.UID, Constants.Secret, awsRegion);

                var translateRequest = new TranslateTextRequest
                {
                    Text = text,
                    SourceLanguageCode = sourceLanguageCode,
                    TargetLanguageCode = targetLanguageCode
                };

                var translateResponse = await translateClient.TranslateTextAsync(translateRequest);

                return translateResponse.TranslatedText;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao traduzir texto: " + ex.Message);
                return null;
            }
        }
    }
}
