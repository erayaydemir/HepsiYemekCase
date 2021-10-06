using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HepsiYemek.WebApi.Models.Abstract
{
    public abstract class BaseResponce
    {
        public bool IsError { get; set; }
        public List<string> Errors { get; set; }
    }
}
