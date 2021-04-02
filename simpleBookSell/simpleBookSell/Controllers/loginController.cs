using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace simpleBookSell.Controllers
{
    public class loginController : Controller
    {
        // GET: login
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string userName, string password)
        {
            if (userName == "admin" && password == "123456")
            {
                //创建登录标识
                Session["isLogin"] = true;
                //判断来源
                if (Session["toUrl"] != null)
                {
                    return new RedirectResult(Session["toUrl"].ToString());
                }
                return new RedirectResult("/store/bookList");
            }
            else
            {
                ViewData["msg"] = "账号或密码不正确！";
                return View();
            }
        }
        public ActionResult Error()
        {
            return View();
        }

    }
}