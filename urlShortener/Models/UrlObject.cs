using Microsoft.Extensions.Logging.Abstractions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

[BsonIgnoreExtraElements]
public class UrlObject
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("url")]
    public string? Url { get; set; } = null!;

    [BsonElement("target")]
    public string? Target { get; set; } = null!;

    [BsonElement("date")]
    public DateTime Date { get; set; }
}