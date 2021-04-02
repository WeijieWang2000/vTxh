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
        public ActionResult Index()
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

    }
}