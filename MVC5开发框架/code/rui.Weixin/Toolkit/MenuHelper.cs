using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace rui.weixin.Toolkit
{
    public class MenuHelper
    {
        public static void createMenu()
        {
            string menuJson = JsonHelper.getMenuJson();
            string value = HttpHelper.Post(UrlHelper.get_Menu_Create(), menuJson);
        }
    }
}
