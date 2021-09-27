using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using UsersManagementMicroservice.Entities;

namespace UsersManagementMicroservice.Helpers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            User user = (User)context.HttpContext.Items["User"];

            if (user == null || user.Role != "admin")
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}
