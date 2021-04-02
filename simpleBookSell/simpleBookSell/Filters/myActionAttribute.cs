using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace simpleBookSell.Filters
{
    public class myActionAttribute : FilterAttribute, IActionFilter, IResultFilter
    {
        private Stopwatch timer;
        //Action执行后执行
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }

        //Action执行前执行
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            timer = new Stopwatch();
            timer.Start();
        }

        public void OnResultExecuted(ResultExecutedContext filterContext)
        {
            timer.Stop();
            filterContext.HttpContext.Response.Write("运行时间：" + timer.ElapsedMilliseconds + "ms");
        }

        public void OnResultExecuting(ResultExecutingContext filterContext)
        {
        }
    }

}