using Base62;

namespace urlShortener.Utils;

public class Utils
{
    public static string GenerateBase62String(string url)
    {
        var base62Converter = new Base62Converter();
        return base62Converter.Encode(url);
    }
    
    public static string SelectRandomString(string urlBase, int length = 5)
    {
        Random random = new Random();
        
        return new string(Enumerable.Repeat(urlBase, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}