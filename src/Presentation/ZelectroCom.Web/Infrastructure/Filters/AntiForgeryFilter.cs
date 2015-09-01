using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace ZelectroCom.Web.Infrastructure.Filters
{
    public class AntiForgeryFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext authorizationContext)
        {
            if (authorizationContext.RequestContext.HttpContext.Request.HttpMethod != "POST")
                return;

            if (HasAttributeOnActionOrController(authorizationContext, typeof(NoAntiForgeryCheckAttribute)))
                return;

            if (HasAttributeOnActionOrController(authorizationContext, typeof(HeaderAntiForgeryAttribute)))
            {
                ValidateRequestHeader(authorizationContext.RequestContext.HttpContext.Request);
            }
            else
            {
                new ValidateAntiForgeryTokenAttribute().OnAuthorization(authorizationContext);
            }
        }

        private void ValidateRequestHeader(HttpRequestBase request)
        {
            string formToken = request.Headers.GetValues("RequestVerificationToken")[0];
            string cookieToken = request.Cookies["__RequestVerificationToken"].Value;
            AntiForgery.Validate(cookieToken, formToken);
        }

        private bool HasAttributeOnActionOrController(AuthorizationContext authorizationContext, Type attributeType)
        {
            return (authorizationContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes(attributeType, true).Length > 0) ||
                (authorizationContext.ActionDescriptor.GetCustomAttributes(attributeType, true).Length > 0);
        }

    }

    public class NoAntiForgeryCheckAttribute : Attribute { }

    public class HeaderAntiForgeryAttribute : Attribute { }
}