using HepsiYemek.DAL.Models.Abstract;
using HepsiYemek.WebApi.Models.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HepsiYemek.WebApi.Models.Concrete
{
    public class ServiceResponce<T>:BaseResponce
        where T:IDocument
    {
        public ServiceResponce()
        {
            Errors = new List<string>();
            Entities = new List<T>();
        }
        public T Entity { get; set; }
        public List<T>  Entities { get; set; }
    }
}
