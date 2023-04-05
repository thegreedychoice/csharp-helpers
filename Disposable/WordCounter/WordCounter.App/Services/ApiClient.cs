using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using WordCounter.App.ServiceReferences;

namespace WordCounter.App.Services
{
    public class ApiClient : IDisposable
    {
        private HttpClient _httpClient = new();

        private readonly IConfiguration _config;
        
        private bool disposedValue;

        public ApiClient(IConfiguration config)
        {
            _config = config;
        }

        public async Task<int> GetWordCount(string input)
        {
            var model = new WordCountModel
            {
                Line = input
            };            
            var apiClient = new WordCounterApiClient(_config["WordCounter:Api:Url"], _httpClient);
            return await apiClient.WordCountAsync(model);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing && _httpClient != null)
                {
                    _httpClient.Dispose();
                    _httpClient = null;
                    //...
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
