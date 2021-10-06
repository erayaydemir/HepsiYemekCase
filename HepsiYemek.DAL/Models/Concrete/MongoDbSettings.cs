using HepsiYemek.DAL.Models.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace HepsiYemek.DAL.Models.Concrete
{
    public class MongoDbSettings : IMongoDbSettings
    {
        public string DatabaseName { get; set; }
        public string ConnectionString { get; set; }
    }
}
