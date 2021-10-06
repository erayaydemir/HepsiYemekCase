using HepsiYemek.DAL.Models.Abstract;
using HepsiYemek.DAL.Models.Concrete.DbObjects;
using HepsiYemek.WebApi.Filters;
using HepsiYemek.WebApi.Models.Concrete;
using HepsiYemek.WebApi.Models.ModelValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HepsiYemek.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IMongoRepository<Category> _categoryRepository;
        public CategoryController(IMongoRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        [HttpGet,Route("GetCategoryByName")]
        [MyException]
        public IActionResult GetCategoryByName([FromQuery]string name)
        {
            var serviceResponce = new ServiceResponce<Category>();

            var categories = _categoryRepository.FilterBy(
                    filter => filter.Name == name
                );

            if (categories == null ||categories.Count() == 0)
            {
                serviceResponce.IsError = true;
                serviceResponce.Errors.Add($"CategoryName : {name} Not Found");
                return NotFound(serviceResponce);
            }
            serviceResponce.Entities = categories.ToList();

            return Ok(serviceResponce);
        }
        [HttpGet,Route("GetCategoryById")]
        [MyException]
        public IActionResult GetCategoryById([FromQuery]string objectId)
        {
            var serviceResponce = new ServiceResponce<Category>();

            if (string.IsNullOrEmpty(objectId))
            {
                serviceResponce.IsError = true;
                serviceResponce.Errors.Add("objectId Can Not Be Null");
                return BadRequest(serviceResponce);
            }

            var category = _categoryRepository.FindById(objectId);

            if (category == null)
            {
                serviceResponce.IsError = true;
                serviceResponce.Errors.Add($"ObjectId : {objectId} Not Found");
                return NotFound(serviceResponce);
            }
            serviceResponce.Entity = category;

            return Ok(serviceResponce);
        }
        [HttpGet]
        [MyException]
        public IActionResult GetAll()
        {
            var serviceResponce = new ServiceResponce<Category>();

            var categories = _categoryRepository.FilterBy(doc => true);

            if (categories == null)
            {
                serviceResponce.IsError = true;
                serviceResponce.Errors.Add("Could Not Find Any Category");
                return NotFound(serviceResponce);
            }

            serviceResponce.Entities = categories.ToList();

            return Ok(serviceResponce);
        }
        [HttpPost]
        [ValidateModel]
        [MyException]
        public IActionResult Post([FromBody] CategoryModel categoryModel)
        {
            var serviceResponce = new ServiceResponce<Category>();
            var document = new Category
            {
                Name = categoryModel.Name,
                Description = categoryModel.Description
            };

            _categoryRepository.InsertOne(document);
            serviceResponce.IsError = false;
            serviceResponce.Entity = document;
            return Ok(serviceResponce);
        }
        [HttpPut("{id}")]
        [ValidateModel]
        [MyException]
        public IActionResult Put(string id,[FromBody] CategoryModel categoryModel)
        {
            var serviceRespnce = new ServiceResponce<Category>();
            if (string.IsNullOrEmpty(id))
            {
                serviceRespnce.IsError = true;
                serviceRespnce.Errors.Add("Update Id Can Not Be Null");
                return BadRequest(serviceRespnce);
            }

            var category = _categoryRepository.FindById(id);
            if(category == null)
            {
                serviceRespnce.IsError = true;
                serviceRespnce.Errors.Add("Could Not Find Any Category For Update");
                return NotFound(serviceRespnce);
            }

            category.Name = categoryModel.Name;
            category.Description = categoryModel.Description;

            _categoryRepository.ReplaceOne(category);

            serviceRespnce.Entity = category;
            return Ok(serviceRespnce);
        }
        [HttpDelete("{id}")]
        [MyException]
        public IActionResult Delete(string id)
        {
            var serviceResponce = new ServiceResponce<Category>();
            var category = _categoryRepository.FindById(id);

            if(category == null)
            {
                serviceResponce.IsError = true;
                serviceResponce.Errors.Add("Delete Model Could Not Find");
                return NotFound(serviceResponce);
            }

            _categoryRepository.DeleteById(category.Id.ToString());
            serviceResponce.Entity = category;
            return Ok(serviceResponce);
        }
    }
}
