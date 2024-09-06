using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static urlShortener.Utils.Utils;
using urlShortener.Database;

namespace urlShortener.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UrlController : ControllerBase
    {
        private readonly DatabaseController _databaseController;
        
        public UrlController()
        {
            _databaseController = new DatabaseController();
        }
        
        [HttpGet]
        public ObjectResult Get(string? shortenedUrl)
        {
            string baseJson = "{\"url\":\"";
            if (shortenedUrl == null)
            {
                return this.StatusCode(400, "Bad request");
            }
            
            try
            {
                var target = _databaseController.Get(shortenedUrl).Target;
                return this.StatusCode(200, baseJson + target + "\"}");
            }
            catch
            {
                return this.StatusCode(404, "Not found");
            }
        }
        
        [HttpPost]
        public ObjectResult Post(string? url, int length = 5)
        {
            if (url == null || length < 4)
            {
                return this.StatusCode(400, "Bad request");
            }
            string baseString = url;
            if (url.Length < 8)
            {
                baseString = url + "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            }
            string shortenedUrl = SelectRandomString(GenerateBase62String(baseString), length);
            
            UrlObject urlObject = new UrlObject
            {
                Url = shortenedUrl,
                Target = url,
                Date = DateTime.Now
            };
            
            _databaseController.Create(urlObject);
            return this.StatusCode(200, urlObject);
        }
    }
}
