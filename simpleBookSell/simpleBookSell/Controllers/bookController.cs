using simpleBookSell.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace simpleBookSell.Controllers
{
    public class bookController : Controller
    {
        // GET: book
        // 首页展示图书列表
        [author]
        [OutputCache(CacheProfile = "mySqlCache")]
        public ActionResult bookList()
        {
            List<db.Books> list = db.bll.books.getBooks();

            return View(list);
        }

        //图书新增界面展示
        [HttpGet]
        public ActionResult insert()
        {
            db.Books entry = new db.Books();
            return View(entry);
        }

        //保存新增图书数据
        [HttpPost]
        public ActionResult insert(string AuthorName, string Title, Nullable<decimal> Price)
        {
            db.bll.books.insert(AuthorName, Title, Price);
            return RedirectToAction("bookList");
        }

        //图书新增界面展示-方法2
        [HttpGet]
        public ActionResult insertEntry()
        {
            db.Books entry = new db.Books();
            return View(entry);
        }

        //保存新增图书数据-方法2
        [HttpPost]
        public ActionResult insertEntry(db.Books entry)
        {
            db.bll.books.insert(entry);
            return RedirectToAction("bookList");
        }

        //图书修改界面展示
        [HttpGet]
        public ActionResult update(int id)
        {
            db.Books entry = db.bll.books.getEntry(id);
            return View(entry);
        }

        //保存修改图书数据
        [HttpPost]
        public ActionResult update(int BookId, string AuthorName, string Title, Nullable<decimal> Price)
        {
            db.bll.books.update(BookId, AuthorName, Title, Price);
            return RedirectToAction("bookList");
        }


        //图书修改界面展示-方法2
        [HttpGet]
        public ActionResult updateEntry(int id)
        {
            db.Books entry = db.bll.books.getEntry(id);
            return View(entry);
        }
        //保存修改图书数据-方法2
        [HttpPost]
        public ActionResult updateEntry(db.Books entry)
        {
            db.bll.books.update(entry);
            return RedirectToAction("bookList");
        }

        //图书修改界面展示-方法3
        [HttpGet]
        public ActionResult update3(int id)
        {
            db.Books entry = db.bll.books.getEntry(id);
            return View(entry);
        }
        //保存修改图书数据-方法3
        [HttpPost]
        public ActionResult update3(db.Books entry)
        {
            db.bll.books.update3(entry);
            return RedirectToAction("bookList");
        }

        //图书删除
        public ActionResult delete(int id)
        {
            db.bll.books.Delete(id);
            return RedirectToAction("bookList");
        }

    }
}