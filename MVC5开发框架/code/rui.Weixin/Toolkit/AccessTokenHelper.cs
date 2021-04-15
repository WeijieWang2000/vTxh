using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace rui.weixin.Toolkit
{
    /// <summary>
    /// AccessToken辅助类
    /// </summary>
    public class AccessTokenHelper
    {
        private System.Web.Caching.Cache objCache = HttpRuntime.Cache;

        //私有构造函数不允许new创建
        private AccessTokenHelper(){}

        //单例模式
        private static AccessTokenHelper instance = null;
        public static AccessTokenHelper getInsance()
        {
            if (instance == null)
                instance = new AccessTokenHelper();
            return instance;
        }

        //获取Access_token值
        public string getToken()
        {
            object value = objCache.Get("access_token");
            rui.logHelper.log("缓存中的token:" + value);
            if(value == null)
            {
                string url = string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", WxConfig.APPID, WxConfig.APPSECRET);
                string tokenJson = HttpHelper.Get(url);
                rui.logHelper.log("获取的tokenJson:" + tokenJson);
                JObject json = JObject.Parse(tokenJson);
                value = json["access_token"].ToString();
                rui.logHelper.log("新获取的token:" + value);
                this.saveToken(value);
            }
            return value.ToString();
        }

        //保存Access_token值
        public void saveToken(object value)
        {
            objCache.Insert("access_token", value, null, DateTime.Now.AddSeconds(7000), System.Web.Caching.Cache.NoSlidingExpiration);
        }
    }
}
