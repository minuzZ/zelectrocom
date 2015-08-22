using System;
using System.Web.Mvc;

namespace ZelectroCom.Web.Infrastructure.Filters
{
    public class AntiForgeryFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext authorizationContext)
        {
            if (authorizationContext.RequestContext.HttpContext.Request.HttpMethod != "POST")
                return;

            if (authorizationContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes(typeof(NoAntiForgeryCheckAttribute), true).Length > 0)
                return;

            if (authorizationContext.ActionDescriptor.GetCustomAttributes(typeof(NoAntiForgeryCheckAttribute), true).Length > 0)
                return;

            new ValidateAntiForgeryTokenAttribute().OnAuthorization(authorizationContext);
        }
    }

    public class NoAntiForgeryCheckAttribute : Attribute
    {
    }
}