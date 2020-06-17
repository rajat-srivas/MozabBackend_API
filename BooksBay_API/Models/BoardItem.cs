using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BooksBay_API.Models
{
	public class BoardItem
	{
		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; }

		[BsonElement("BoardItem")]
		public string Item { get; set; }

		[BsonElement("BoardId")]
		public string BoardId { get; set; }


	}
}