using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace db.bll
{
    public class orders
    {
        //获取订单列表（orderList）
        public static List<db.sv_Orders> getOrders()
        {
            dbEntities dc = new dbEntities();
            List<sv_Orders> list = (from a in dc.sv_Orders
                                    select a).ToList<sv_Orders>();
            return list;
        }

        //新增订单（order-post)
        public static void insert(int bookId, int num, string address)
        {
            dbEntities dc = new dbEntities();
            Orders entry = new Orders();
            entry.BookId = bookId;
            entry.Num = num;
            entry.Address = address;
            dc.Orders.Add(entry);
            dc.SaveChanges();
        }
    }
}

