using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using WordCounter.ModernApp.Data;
using WordCounter.ModernApp.Data.Model;
using WordCounter.ModernApp.Services;

namespace WordCounter.ModernApp.Processors
{
    public class BookLineProcessor
    {
        private readonly BookFeedContext _context;
        private readonly ApiClient _apiClient;
        private readonly ILogger _logger;

        public BookLineProcessor(BookFeedContext context, ApiClient apiClient, ILogger<BookLineProcessor> logger)
        {
            _context = context;
            _apiClient = apiClient;
            _logger = logger;
        }

        public ApiClient ApiClient
        {
            get => default;
            set
            {
            }
        }

        public BookFeedContext BookFeedContext
        {
            get => default;
            set
            {
            }
        }

        public async Task<int> GetWordCount(string path, int lineNumber, string line, CancellationTokenSource cancellationTokenSource)
        {
            var wordCount = 0;
            cancellationTokenSource.Token.ThrowIfCancellationRequested();
            try
            {
                var excerpt = line.Length > 100 ? line.Substring(0, 100) : line;
                _logger.LogInformation("Processing line: {0}, '{1}...'", lineNumber, excerpt);
                wordCount = await _apiClient.GetWordCount(line);

                var bookFeed = await _context.BookFeeds.FirstAsync(x => x.Path == path);
                bookFeed.BookLines.Add(new BookLine
                {
                    LineNumber = lineNumber,
                    WordCount = wordCount,
                    Excerpt = excerpt
                });
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("ERROR: " + ex.Message);
                cancellationTokenSource.Cancel();
            }
            return wordCount;
        }
    }
}
