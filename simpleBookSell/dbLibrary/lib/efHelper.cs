using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace db.lib
{
    public class efHelper
    {
        public static void updateEntry(Object entry, dbEntities dc)
        {
            dc.Entry(entry).State = System.Data.Entity.EntityState.Unchanged;

            //获取前端表单名称
            List<string> list = System.Web.HttpContext.
                Current.Request.Form.AllKeys.ToList<string>();

            foreach (var p in entry.GetType().GetProperties())
            {
                if (list.Contains(p.Name))
                    dc.Entry(entry).Property(p.Name).IsModified = true;
            }
        }
    }
}
