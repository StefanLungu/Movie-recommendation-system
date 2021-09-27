using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PredictionMicroservice.Models;
using System;

namespace PredictionMicroservice.Helpers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class RecommendationAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            GetUserDTO user = (GetUserDTO)context.HttpContext.Items["User"];
            int userId = int.Parse(context.HttpContext.Request.Path.Value.Split('/')[^1]);

            if (user == null || user.Id != userId)
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}
