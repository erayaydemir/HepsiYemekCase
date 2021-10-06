using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HepsiYemek.WebApi.Models.ModelValidation
{
    public class CategoryModel
    {
        [DisplayName("name"),Required(ErrorMessage ="{0} Field Required. Can Not Be Null Or Empty")]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
