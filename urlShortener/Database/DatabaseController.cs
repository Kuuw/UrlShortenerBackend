using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using urlShortener.Utils;

namespace urlShortener.Database;

public class DatabaseController
{
    private readonly IMongoCollection<UrlObject> _urlObjects;

    public DatabaseController()
    {
        var client = new MongoClient(Config.ATLAS_URI);
        var database = client.GetDatabase("urlShortener");
        _urlObjects = database.GetCollection<UrlObject>("urls");
    }

    public UrlObject Get(string id)
    {
        var filter = Builders<UrlObject>.Filter.Eq(url => url.Url, id);
        
        return _urlObjects.Find<UrlObject>(filter).FirstOrDefault();
    }

    public UrlObject Create(UrlObject urlObject)
    {
        _urlObjects.InsertOne(urlObject);
        return urlObject;
    }

    public void Update(string id, UrlObject urlObjectIn) =>
        _urlObjects.ReplaceOne(urlObject => urlObject.Id == id, urlObjectIn);

    public void Remove(UrlObject urlObjectIn) =>
        _urlObjects.DeleteOne(urlObject => urlObject.Id == urlObjectIn.Id);

    public void Remove(string id) =>
        _urlObjects.DeleteOne(urlObject => urlObject.Id == id);
    
}