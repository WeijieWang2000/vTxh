using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace simpleBookSell.Filters
{
    public class authorAttribute : FilterAttribute, IAuthorizationFilter
    {
        //完成登录检测
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            ////未登录
            //if (filterContext.HttpContext.Session["isLogin"] == null)
            //{
            //    string url = filterContext.HttpContext.Request.RawUrl;
            //    filterContext.HttpContext.Session["toUrl"] = url;
            //    filterContext.Result = new RedirectResult("/login/Index");
            //}

            //string controllerName = filterContext.Controller.ToString();
            //string actionName = filterContext.ActionDescriptor.ActionName;
            //if (controllerName == "simpleBookSell.Controllers.loginController")
            //    return;
        }
    }

}