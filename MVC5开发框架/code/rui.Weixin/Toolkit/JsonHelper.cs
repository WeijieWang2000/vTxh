using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace rui.weixin.Toolkit
{
    public class JsonHelper
    {
        //获取要发送的模板JSON格式
        public static string getFileJson(string fileName)
        {
            FileStream fs = new FileStream(HttpContext.Current.Server.MapPath(string.Format("~/json/{0}.txt", fileName)), FileMode.Open);
            StreamReader sr = new StreamReader(fs, Encoding.GetEncoding("UTF-8"));
            string jsonContent = sr.ReadToEnd();
            sr.Close();
            return jsonContent;
        }

        //获取菜单的JSON
        public static string getMenuJson()
        {
            string json = JsonHelper.getFileJson("createMenu");
            return string.Format(json);
        }
    }
}
