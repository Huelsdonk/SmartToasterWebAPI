using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace ToastApiReact.Models
{
    public class Toaster
    {
        [BsonId]
        // this sets ID as primary key
        [BsonRepresentation(BsonType.ObjectId)]
        // this allows for the parameter to be passed as string. Mongo
        // handles the conversion
        public string Id { get; set; }

        [BsonElement("Name")]
        // this attribute sets the property "Name" in the DB Collection
        // So, ToasterName occupies "Name" property in DB
        [JsonProperty("Name")]
        // This tells the serializer to all serialize the "Name" property
        // so that the bookName property is return as Name
        public string ToasterName { get; set; }

        public bool On { get; set; }

        public int Heat { get; set; }

        public int Time { get; set; }
    }
}

