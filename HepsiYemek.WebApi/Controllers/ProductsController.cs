using HepsiYemek.DAL.Models.Abstract;
using HepsiYemek.DAL.Models.Concrete.DbObjects;
using HepsiYemek.WebApi.Filters;
using HepsiYemek.WebApi.Models.Concrete;
using HepsiYemek.WebApi.Models.ModelValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HepsiYemek.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IMongoRepository<Category> _categoryRepository;
        private readonly IMongoRepository<Products> _productsRepository;

        private readonly IDistributedCache _distributedCache;
        public ProductsController(IMongoRepository<Category> categoryRepository,IMongoRepository<Products> productsRepository,IDistributedCache distributedCache)
        {
            _categoryRepository = categoryRepository;
            _productsRepository = productsRepository;
            _distributedCache = distributedCache;
        }
        [HttpGet, Route("GetProductsByName")]
        [MyException]
        public IActionResult GetProductsByName([FromQuery] string name)
        {
            var serviceResponce = new ServiceResponce<Products>();

            if (string.IsNullOrEmpty(name))
            {
                serviceResponce.IsError = true;
                serviceResponce.Errors.Add("Name Param Can Not Be Null");
                return BadRequest(serviceResponce);
            }

            var products = _productsRepository.FilterBy(
                    filter => filter.Name == name
                ).ToList();

            if(products == null || products.Count == 0)
            {
                serviceResponce.IsError = true;
                serviceResponce.Errors.Add($"ProductName : {name} Not Found");
                return NotFound(serviceResponce);
            }

            foreach (var product in products)
            {
                var productCategory = _categoryRepository.FindById(product.CategoryId);
                product.Category = productCategory;
            }
            serviceResponce.Entities = products;
            return Ok(serviceResponce);
        }
        [HttpGet, Route("GetProductById")]
        [MyException]
        public IActionResult GetProductById([FromQuery] string objectId)
        {
            var serviceResponce = new ServiceResponce<Products>();

            if (string.IsNullOrEmpty(objectId))
            {
                serviceResponce.IsError = true;
                serviceResponce.Errors.Add("ObjectId Can Not Be Null");
                return BadRequest(serviceResponce);
            }

            var product = _productsRepository.FindById(objectId);

            if (product == null)
            {
                serviceResponce.IsError = true;
                serviceResponce.Errors.Add($"ObjectId : {objectId} Not Found");
                return NotFound(serviceResponce);
            }

            var productCategory = _categoryRepository.FindById(product.CategoryId);
            product.Category = productCategory;

            serviceResponce.Entity = product;

            return Ok(serviceResponce);
        }
        [HttpGet,Route("GetProductByIdWithRedis")]
        [MyException]
        public async Task<IActionResult> GetProductByIdWithRedis([FromQuery] string id)
        {
            var serviceResponce = new ServiceResponce<Products>();
            var product = await GetProductFromRedis(id);
            if(product == null)
            {
                serviceResponce.IsError = true;
                serviceResponce.Errors.Add("Could Not Find Any Product");
                return NotFound(serviceResponce);
            }

            serviceResponce.Entity = product;
            return Ok(serviceResponce);
        }
        private async Task<Products> GetProductFromRedis(string id)
        {
            var cacheKey = id;

            var encodedCacheProduct = await _distributedCache.GetAsync(cacheKey);
            if(encodedCacheProduct != null)
            {
                var serializedProduct = Encoding.UTF8.GetString(encodedCacheProduct);
                var product = JsonConvert.DeserializeObject<Products>(serializedProduct);
                return product;
            }
            else
            {
                var product = await _productsRepository.FindByIdAsync(id);
                if (product != null)
                {
                    var category = await _categoryRepository.FindByIdAsync(product.CategoryId);
                    product.Category = category;
                    var serializedProduct = JsonConvert.SerializeObject(product);
                    var encodedProduct = Encoding.UTF8.GetBytes(serializedProduct);
                    var options = new DistributedCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                        .SetAbsoluteExpiration(DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc).AddMinutes(5));
                    await _distributedCache.SetAsync(cacheKey, encodedProduct, options);
                    return product;
                }
                else
                    return null;
            }
        }

        [HttpGet]
        [MyException]
        public IActionResult GetAll()
        {
            var serviceResponce = new ServiceResponce<Products>();

            var products = _productsRepository.FilterBy(doc => true).ToList();

            if (products == null)
            {
                serviceResponce.IsError = true;
                serviceResponce.Errors.Add("Could Not Find Any Product");
                return NotFound(serviceResponce);
            }

            foreach (var product in products)
            {
                var productCategory = _categoryRepository.FindById(product.CategoryId);
                product.Category = productCategory;
            }

            serviceResponce.Entities = products;

            return Ok(serviceResponce);
        }

        [HttpPost]
        [ValidateModel]
        [MyException]
        public IActionResult Post([FromBody] ProductsModel productsModel)
        {
            var serviceResponce = new ServiceResponce<Products>();

            var productCategory = _categoryRepository.FindById(productsModel.CategoryId);

            if(productCategory == null)
            {
                serviceResponce.IsError = true;
                serviceResponce.Errors.Add("Product's Category Could Not Find. Please Enter Existed Category ObjectId");
                return NotFound(serviceResponce);
            }

            var product = new Products
            {
                Name = productsModel.Name,
                Description = productsModel.Description,
                CategoryId = productsModel.CategoryId,
                Currency = productsModel.Currency,
                Price = productsModel.Price
            };
            _productsRepository.InsertOne(product);
            product.Category = productCategory;
            serviceResponce.Entity = product;
            return Ok(serviceResponce);
        }
        [HttpPut("{id}")]
        [ValidateModel]
        [MyException]
        public IActionResult Put(string id, [FromBody] ProductsModel productModel)
        {
            var serviceResponce = new ServiceResponce<Products>();

            #region optionalCodeBlock
            var hexObjectIdValidate = new HexObjectIdAttribute("From Query Id Parameter Could Be HexaDecimal Format");
            if (!hexObjectIdValidate.IsValid(id))
            {
                serviceResponce.IsError = true;
                serviceResponce.Errors.Add(hexObjectIdValidate.ErrorMessage);
                return BadRequest(serviceResponce);
            }
            #endregion

            var product = _productsRepository.FindById(id);
            if(product == null)
            {
                serviceResponce.IsError = true;
                serviceResponce.Errors.Add("Could Not Find Any Product For Update");
                return NotFound(serviceResponce);
            }

            var productCategory = _categoryRepository.FindById(productModel.CategoryId);

            if(productCategory == null)
            {
                serviceResponce.IsError = true;
                serviceResponce.Errors.Add("Could Not Find Any Category For Update Product");
                return NotFound(serviceResponce);
            }

            //TODO: You Must Remove Modified ObjectId From Redis Server
            _distributedCache.RemoveAsync(product.Id.ToString());

            product.Name = productModel.Name;
            product.Description = productModel.Description;
            product.Currency = productModel.Currency;
            product.Price = productModel.Price;
            product.CategoryId = productModel.CategoryId;
            _productsRepository.ReplaceOne(product);
            product.Category = productCategory;

            serviceResponce.Entity = product;

            return Ok(serviceResponce);
        }
        [HttpDelete("{id}")]
        [MyException]
        public IActionResult Delete(string id)
        {
            var serviceResponce = new ServiceResponce<Products>();

            var deleteProduct = _productsRepository.FindById(id);
            if(deleteProduct == null)
            {
                serviceResponce.IsError = true;
                serviceResponce.Errors.Add("Could Not Find Any Product For Delete");
                return NotFound(serviceResponce);
            }

            _productsRepository.DeleteById(deleteProduct.Id.ToString());
            serviceResponce.Entity = deleteProduct;
            return Ok(serviceResponce);
        }
    }
}
