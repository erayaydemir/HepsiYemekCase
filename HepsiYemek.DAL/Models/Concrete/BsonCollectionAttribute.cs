using System;
using System.Collections.Generic;
using System.Text;

namespace HepsiYemek.DAL.Models.Concrete
{
    [AttributeUsage(AttributeTargets.Class,Inherited =false)]
    public class BsonCollectionAttribute :Attribute
    {
        public string CollectionName { get; }
        public BsonCollectionAttribute(string collectionName)
        {
            CollectionName = collectionName;
        }
    }
}
