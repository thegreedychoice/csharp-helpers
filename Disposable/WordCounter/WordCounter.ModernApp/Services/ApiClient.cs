using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using WordCounter.ModernApp.ServiceReferences;

namespace WordCounter.ModernApp.Services
{
    public class ApiClient 
    {
        private readonly IConfiguration _config;
        private IHttpClientFactory _httpClientFactory;

        public ApiClient(IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            _httpClientFactory = httpClientFactory;
            _config = config;
        }

        public async Task<int> GetWordCount(string input)
        {
            var model = new WordCountModel
            {
                Line = input
            };

            // HttpClientFactory manages connections -
            // HttpClient is disposable but you don't need to dispose it

            // ...
            var httpClient = _httpClientFactory.CreateClient();
            var apiClient = new WordCounterApiClient(_config["WordCounter:Api:Url"], httpClient);
            return await apiClient.WordCountAsync(model);
        }
    }
}
