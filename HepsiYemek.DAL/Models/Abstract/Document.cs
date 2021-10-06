using HepsiYemek.DAL.Models.Concrete.DbObjects;
using MongoDB.Bson;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HepsiYemek.DAL.Models.Abstract
{
    public abstract class Document : IDocument
    {
        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId Id { get; set; }

        public DateTime CreateDate => Id.CreationTime;
    }
}
