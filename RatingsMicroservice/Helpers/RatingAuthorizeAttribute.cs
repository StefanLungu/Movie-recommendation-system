using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json.Linq;
using RatingsMicroservice.Models;
using System;
using System.IO;

namespace RatingsMicroservice.Helpers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class RatingAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public async void OnAuthorization(AuthorizationFilterContext context)
        {
            GetUserDTO user = (GetUserDTO)context.HttpContext.Items["User"];

            context.HttpContext.Request.EnableBuffering();
            var body = await new StreamReader(context.HttpContext.Request.Body).ReadToEndAsync();
            JObject jsonObject = JObject.Parse(body);
            int userId = int.Parse((string)jsonObject["userId"]);

            if (user == null || user.Id != userId)
            {
                context.Result = new UnauthorizedResult();
            }

            context.HttpContext.Request.Body.Seek(0, SeekOrigin.Begin);
        }
    }
}
