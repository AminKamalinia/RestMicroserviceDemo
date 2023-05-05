using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EmployeeWebAPI.Models.Entities
{
    [Serializable, BsonIgnoreExtraElements]
    public class Department
    {
        [BsonId, BsonElement("_Id"), BsonRepresentation(BsonType.ObjectId)]
        public string _Id { get; set; }

        [BsonElement("Name"), BsonRepresentation(BsonType.String)]
        public string Name { get; set; }

        [BsonElement("IsDeleted"), BsonRepresentation(BsonType.Boolean)]
        public bool IsDeleted { get; set; }

    }
}