/*
 Usage:
 <form action="" method="post">
     <input type="submit" value="Save" name="action:Save" />
     <input type="submit" value="Cancel" name="action:Cancel" />
 </form>
 *
    [HttpPost]
    [MultipleButton(Name = "action", Argument = "Save")]
    public ActionResult Save(MessageModel mm) { ... }

    [HttpPost]
    [MultipleButton(Name = "action", Argument = "Cancel")]
    public ActionResult Cancel(MessageModel mm) { ... }
 */
using System;
using System.Reflection;
using System.Web.Mvc;

namespace ZelectroCom.Web.Infrastructure.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class MultipleButtonAttribute : ActionNameSelectorAttribute
    {
        public override bool IsValidName(ControllerContext controllerContext, string actionName, MethodInfo methodInfo)
        {
            if (actionName.Equals(methodInfo.Name, StringComparison.InvariantCultureIgnoreCase))
                return true;

            if (!actionName.Equals("Action", StringComparison.InvariantCultureIgnoreCase))
                return false;

            var request = controllerContext.RequestContext.HttpContext.Request;
            return request[methodInfo.Name] != null;
        }
    }
}