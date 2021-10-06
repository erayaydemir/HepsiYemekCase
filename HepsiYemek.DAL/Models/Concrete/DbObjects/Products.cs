using HepsiYemek.DAL.Models.Abstract;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace HepsiYemek.DAL.Models.Concrete.DbObjects
{
    [BsonCollection("products")]
    public class Products:Document
    {
        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("price")]
        [BsonRepresentation(BsonType.Decimal128)]
        public decimal Price { get; set; }

        [BsonElement("currency")]
        public string Currency { get; set; }

        [BsonElement("categoryId")]
        public string CategoryId { get; set; }
        [BsonIgnore()]
        public Category Category { get; set; }
    }
}
