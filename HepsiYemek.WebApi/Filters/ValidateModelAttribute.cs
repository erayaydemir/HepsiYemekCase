using HepsiYemek.DAL.Models.Abstract;
using HepsiYemek.DAL.Models.Concrete.DbObjects;
using HepsiYemek.WebApi.Models.Concrete;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HepsiYemek.WebApi.Filters
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var respoce = new ServiceResponce<Document>();
                respoce.IsError = true;
                respoce.Errors = context.ModelState.Values
                    .SelectMany(e => e.Errors)
                    .Select(r => r.ErrorMessage).ToList();

                context.Result = new BadRequestObjectResult(respoce);
            }
        }
    }
}
