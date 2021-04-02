using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace db.bll
{
    public class books
    {
        //获取图书列表（bookList）
        public static List<db.Books> getBooks()
        {
            dbEntities dc = new dbEntities();
            List<Books> list = dc.Books.ToList<Books>();
            return list;
        }

        //获取图书详情（order-get）
        public static db.Books getEntry(int bookId)
        {
            dbEntities dc = new dbEntities();
            Books entry = dc.Books.SingleOrDefault(a => a.BookId == bookId);
            return entry;
        }

        //新增图书
        public static void insert(string AuthorName, string Title, Nullable<decimal> Price)
        {
            dbEntities dc = new dbEntities();
            Books entry = new Books();
            entry.AuthorName = AuthorName;
            entry.Title = Title;
            entry.Price = Price;

            dc.Books.Add(entry);

            dc.SaveChanges();
        }

        //新增图书-方法2
        public static void insert(db.Books entry)
        {
            dbEntities dc = new dbEntities();

            dc.Books.Add(entry);

            dc.SaveChanges();
        }

        //修改图书
        public static void update(int BookId, string AuthorName, string Title, Nullable<decimal> Price)
        {
            dbEntities dc = new dbEntities();

            Books entry = dc.Books.SingleOrDefault(a => a.BookId == BookId);

            entry.AuthorName = AuthorName;
            entry.Title = Title;
            entry.Price = Price;

            dc.SaveChanges();
        }

        //修改图书-方法2
        public static void update(db.Books entry)
        {
            dbEntities dc = new dbEntities();

            dc.Entry<db.Books>(entry).State = System.Data.Entity.EntityState.Modified;

            dc.SaveChanges();
        }

        //修改图书-方法3
        public static void update3(db.Books entry)
        {
            dbEntities dc = new dbEntities();

            db.lib.efHelper.updateEntry(entry, dc);

            dc.SaveChanges();
        }

        //删除图书
        public static void Delete(int id)
        {
            dbEntities dc = new dbEntities();
            Books entry = dc.Books.SingleOrDefault(a => a.BookId == id);
            dc.Books.Remove(entry);
            dc.SaveChanges();
        }

        public static List<db.Books> getCheapBooks()
        {
            dbEntities dc = new dbEntities();
            List<Books> list = dc.Books.Where(a => a.Price < 20).ToList<Books>();
            return list;
        }
        public static List<db.Books> getExpensiveBooks()
        {
            dbEntities dc = new dbEntities();
            List<Books> list = dc.Books.Where(a => a.Price >= 20).ToList<Books>();
            return list;
        }

    }
}