using HepsiYemek.DAL.Models.Abstract;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace HepsiYemek.DAL.Models.Concrete.DbObjects
{
    [BsonCollection("categories")]
    public class Category:Document
    {
        [BsonElement("name")]
        public string Name { get; set; }
        [BsonElement("description")]
        public string Description { get; set; }
    }
}
