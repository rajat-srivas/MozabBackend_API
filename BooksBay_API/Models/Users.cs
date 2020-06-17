using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Taskboard_API.Models
{
    public class Users
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Name")]
        public string Name { get; set; }

        [BsonElement("Email")]
        public string Email { get; set; }
        
        [BsonIgnore]
        public string Password { get; set; }

        [BsonElement("PasswordHash")]
        public byte[] PasswordHash { get; set; }

        [BsonElement("Salt")]
        public byte[] Salt { get; set; }

    }
}