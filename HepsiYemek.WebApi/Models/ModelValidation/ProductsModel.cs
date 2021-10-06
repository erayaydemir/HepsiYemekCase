using HepsiYemek.WebApi.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HepsiYemek.WebApi.Models.ModelValidation
{
    public class ProductsModel
    {
        [DisplayName("name"),Required(ErrorMessage = "{0} Field Required. Can Not Be Null Or Empty")]
        public string Name { get; set; }
        public string Description { get; set; }

        [DisplayName("categoryId"),Required(ErrorMessage = "{0} Feild Required. Can Not Be Null Or Empty")]
        [HexObjectId("categoryId Could Be Hexadecimal Format")]
        public string CategoryId { get; set; }

        [DisplayName("price"),Required(ErrorMessage = "{0} Field Required. Can Not Be Null Or Empty")]
        [Range(0.1,5000,ErrorMessage ="{0} Field Min : {1} Max : {2}")]
        public decimal Price { get; set; }

        [DisplayName("currency"),Required(ErrorMessage = "{0} Field Required. Can Not Be Null Or Empty")]
        [EnumDataType(typeof(CurrencyEnumType),ErrorMessage ="{0} Field Can Only TL Or USD")]
        public string Currency { get; set; }

    }

    public enum CurrencyEnumType
    {
        [Display(Name = "TL")]
        TL,
        [Display(Name = "USD")]
        USD
    }
}
