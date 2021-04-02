using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace simpleBookSell.Filters
{
    public class myErrorAttribute : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            string msg = "";
            if (filterContext.Exception != null)
            {
                msg += filterContext.Exception.Message;
            }
            if (filterContext.Exception.InnerException != null)
            {
                msg += filterContext.Exception.InnerException.Message;
            }

            filterContext.HttpContext.Session["errorMsg"] = msg;
            //异常处理标识置为真
            filterContext.ExceptionHandled = true;
            filterContext.Result = new RedirectResult("/login/error");
        }

    }
}