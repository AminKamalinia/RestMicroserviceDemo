using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EmployeeWebAPI.Models.Entities
{
    [Serializable, BsonIgnoreExtraElements]
    public class Employee
    {
        [BsonId, BsonElement("_Id"), BsonRepresentation(BsonType.ObjectId)]
        public string _Id { get; set; }

        [BsonElement("FullName"), BsonRepresentation(BsonType.String)]
        public string FullName { get; set; }

        [BsonElement("Email"), BsonRepresentation(BsonType.String)]
        public string Email { get; set; }

        [BsonElement("Password"), BsonRepresentation(BsonType.String)]
        public string Password { get; set; }

        [BsonElement("CellphoneNumber"), BsonRepresentation(BsonType.String)]
        public string CellphoneNumber { get; set; }

        [BsonElement("PersonalPictureFileName"), BsonRepresentation(BsonType.String)]
        public string PersonalPictureFileName { get; set; }

        [BsonElement("IsActive"), BsonRepresentation(BsonType.Boolean)]
        public bool IsActive { get; set; }

        [BsonElement("DepartmentIds")]
        public List<string> DepartmentIds { get; set; }

        [BsonElement("IsDeleted"), BsonRepresentation(BsonType.Boolean)]
        public bool IsDeleted { get; set; }
    }
}