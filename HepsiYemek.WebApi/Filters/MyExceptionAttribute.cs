using HepsiYemek.DAL.Models.Abstract;
using HepsiYemek.WebApi.Models.Concrete;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HepsiYemek.WebApi.Filters
{
    public class MyExceptionAttribute:ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            var serviceResponce = new ServiceResponce<Document>();
            serviceResponce.IsError = true;
            serviceResponce.Errors.Add($"An Exception Catched Message : {context.Exception.Message}");
            serviceResponce.Errors.Add($"Stack Trace : {context.Exception.StackTrace}");

            context.Result = new BadRequestObjectResult(serviceResponce);
        }
    }
}
