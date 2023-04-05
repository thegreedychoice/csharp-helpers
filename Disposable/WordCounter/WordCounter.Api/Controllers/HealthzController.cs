using Microsoft.AspNetCore.Mvc;

namespace WordCounter.Api.Controllers
{
    public class HealthzController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Ok();
        }
    }
}
