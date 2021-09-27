using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MoviesMicroservice.Models;
using System;

namespace MoviesMicroservice.Helpers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            GetUserDTO user = (GetUserDTO)context.HttpContext.Items["User"];

            if (user == null || user.Role != "admin")
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}
