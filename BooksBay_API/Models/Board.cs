using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace BooksBay_API.Models
{
	public class Board
	{
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("BoardTitle")]
        public string BoardTitle { get; set; }

        [BsonElement("LinkedUser")]
        public string LinkedUser { get; set; }
        [BsonIgnore]
        public List<BoardItem> BoardItems { get; set; }
    }

}