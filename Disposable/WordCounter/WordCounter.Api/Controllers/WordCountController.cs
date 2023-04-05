using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using WordCounter.Api.Models;

namespace WordCounter.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WordCountController : ControllerBase
    {
        private readonly ILogger<WordCountController> _logger;

        public WordCountController(ILogger<WordCountController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
        public IActionResult Get(WordCountModel input)
        {
            var count = 0;
            if (!string.IsNullOrEmpty(input.Line))
            {
                count = input.Line.Split(' ').Length;
            }
            _logger.LogDebug($"Counted: {count} words in line");
            return Ok(count);
        }
    }
}
