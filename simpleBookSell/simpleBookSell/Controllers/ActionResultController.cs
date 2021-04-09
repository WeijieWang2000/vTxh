using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace simpleBookSell.Controllers
{
    public class ActionResultController : Controller
    {
        // GET: ActionResult
        public ActionResult Txh()
        {
            return View();
        }
        public ActionResult DisplayCheapBooks()
        {
            List<db.Books> list = db.bll.books.getCheapBooks();
            return PartialView(list);
        }
        public ActionResult DisplayExpenBooks()
        {
            List<db.Books> list = db.bll.books.getExpensiveBooks();
            return PartialView(list);
        }
        
        public ActionResult css(string color)
        {
            //if (color == "red")
            //{
            //    return Content("body{color:red}", "text/css");
            //}
            //else if (color == "blue")
            //{
            //    return Content("body{color:red}", "text/css");
            //}
            return Content("body{color:" + color + "}", "text/css");
        }

        public ActionResult DownloadFile()
        {
            return File(Server.MapPath("~/123.txt"), "text/plain", "我的文件.txt");
        }

        public ActionResult transJSon()
        {
            List<db.Books> list = db.bll.books.getBooks();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult js()
        {
            return JavaScript("alert('1111111')");
        }
    }
}