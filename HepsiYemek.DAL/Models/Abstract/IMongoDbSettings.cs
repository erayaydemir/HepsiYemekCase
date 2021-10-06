using System;
using System.Collections.Generic;
using System.Text;

namespace HepsiYemek.DAL.Models.Abstract
{
    public interface IMongoDbSettings
    {
        string DatabaseName { get; set; }
        string ConnectionString { get; set; }
    }
}
