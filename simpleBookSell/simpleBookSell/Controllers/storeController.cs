using simpleBookSell.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace simpleBookSell.Controllers
{
    public class storeController : Controller
    {
        // 首页展示图书列表
        [author]
        [OutputCache(Duration = 60)]
        public ActionResult bookList()
        {
            List<db.Books> list = db.bll.books.getBooks();

            return View(list);
        }

        // 购书
        [HttpGet]
        [OutputCache(CacheProfile = "myCache")]
        public ActionResult order(int id)
        {
            db.Books entry = db.bll.books.getEntry(id);

            return View(entry);
        }

        // 下单确认购买
        [HttpPost]
        public ActionResult order(int bookId, int num, string address)
        {
            db.bll.orders.insert(bookId, num, address);

            return RedirectToAction("orderList");
        }

        //订单列表
        public ActionResult orderList()
        {
            List<db.sv_Orders> list = db.bll.orders.getOrders();
            return View(list);
        }
    }
}