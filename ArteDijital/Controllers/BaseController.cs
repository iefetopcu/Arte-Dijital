using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ArteDijital.Models.Entity;

namespace ArteDijital.Controllers
{
    public class BaseController : Controller
    {

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var session = (usertable)filterContext.HttpContext.Session["MySessionUser"];
            if (session.userauthorityID == 1 || session.userauthorityID == 2)
            {
                base.OnActionExecuting(filterContext);
                return;

            }
            else
            {
                filterContext.Result = new RedirectResult("/");
                base.OnActionExecuting(filterContext);
            }

            filterContext.Result = new RedirectResult("/");
            base.OnActionExecuting(filterContext);
        }

    }
}