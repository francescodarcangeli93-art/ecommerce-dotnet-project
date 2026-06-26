using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebEcommerce.Filters
{
    public class CheckAdminAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var role = filterContext.HttpContext.Session["Role"];

            if (role == null || role.ToString().ToLower() == "customer")
            {
                filterContext.Result = new ViewResult { ViewName = "Error" };
            }

            base.OnActionExecuting(filterContext);
        }
    }
}