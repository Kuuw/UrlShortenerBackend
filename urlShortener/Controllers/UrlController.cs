using System.Net;
using System.Text.RegularExpressions;
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
        public ObjectResult Post(string? url, int length = 5, string customShortenedUrl = "")
        {
            string shortenedUrl = "";
            if (url == null || length < 4 || length > 10)
            {
                return this.StatusCode(400, "Bad request");
            }
            
            if (customShortenedUrl == "")
            {
                string baseString = url;
                if (url.Length < 8)
                {
                    baseString = url + "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                }
                while (_databaseController.Get(shortenedUrl) != null)
                {
                    shortenedUrl = SelectRandomString(GenerateBase62String(baseString), length);
                }
            }
            else
            {
                string customShortenedUrlRegex = "^[a-zA-Z0-9]*$";
                
                if (customShortenedUrl.Length < 4 || customShortenedUrl.Length > 10 || !Regex.IsMatch(customShortenedUrl, customShortenedUrlRegex))
                {
                    return this.StatusCode(400, "Bad request");
                }
                if (_databaseController.Get(customShortenedUrl) != null)
                {
                    return this.StatusCode(409, "Conflict");
                }
                shortenedUrl = customShortenedUrl;
            }
            
            
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
