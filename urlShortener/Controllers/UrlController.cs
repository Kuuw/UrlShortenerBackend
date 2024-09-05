using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static urlShortener.Utils.Utils;

namespace urlShortener.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UrlController : ControllerBase
    {
        [HttpGet]
        public string Get(string url, int length = 5)
        {
            return SelectRandomString(url, length);
        }
    }
}
