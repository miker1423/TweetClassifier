using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

using TweetClassifer.Shared.Models;

namespace TweetClassifer.Web.Models
{
    public class TweetEntry
    {
        [BsonId(IdGenerator = typeof(ObjectIdGenerator))]
        public ObjectId ID { get; set; }

        public TweetVM Tweet { get; set; }

        public bool IsClassified { get; set; }
    }
}
