using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace rui.weixin.jsSDK
{
    public class JSSDK
    {
        private string appId;
        private string appSecret;
        private bool _debug;
        private SimpleCacheProvider _cache;


        const string URL_FORMAT_TOKEN = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}";
        const string URL_FORMAT_TICKET = "https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={0}&type=jsapi";

        const string CACHE_TOKEN_KEY = "CACHE_TOKEN_KEY";
        const string CACHE_TICKET_KEY = "CACHE_TICKET_KEY";

        /// <summary>
        /// 构建JDK对象
        /// </summary>
        /// <param name="appId">微信公众账号的AppId</param>
        /// <param name="appSecret">公众账号的appSecret</param>
        /// <param name="debug">是否调试</param>
        public JSSDK(string appId, string appSecret, bool debug = false)
        {
            this.appId = appId;
            this.appSecret = appSecret;
            this._cache = SimpleCacheProvider.GetInstance();
            this._debug = debug;
        }

        /// <summary>
        /// 生成签名的随机字符串
        /// </summary>
        /// <param name="length">长度，小于32位，默认16位</param>
        /// <returns>随机字符串</returns>
        private string CreateNonceStr(int length = 16)
        {
            return Guid.NewGuid().ToString().Replace("-", "").Substring(0, length > 32 ? 32 : length);
        }


        /// <summary>
        /// 获取AccessToken
        /// </summary>
        /// <returns>AccessToken</returns>
        private string GetAccessToken()
        {
            var token = this._cache.GetCache(CACHE_TOKEN_KEY);
            rui.logHelper.log("-------------1-------" + token);
            if (token != null)
                return token.ToString();

            try
            {
                string result = HttpGet(string.Format(URL_FORMAT_TOKEN, this.appId, this.appSecret));
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Dictionary<string, object> jsonObj = serializer.Deserialize<dynamic>(result);
                if (jsonObj.ContainsKey("access_token"))
                {
                    token = jsonObj["access_token"].ToString();
                    rui.logHelper.log("-------------2-------" + token);
                    this._cache.SetCache(CACHE_TOKEN_KEY, token.ToString(), 7000);
                }
                else
                {
                    //为了程序正常运行，不抛出错误，可以记录日志
                    token = jsonObj["errmsg"];
                }
            }
            catch
            {
                //为了程序正常运行，不抛出错误，可以记录日志
                token = "there_is_an_error_when_getting_token";
            }
            rui.logHelper.log("-------------3-------" + token);
            return token.ToString();
        }

        /// <summary>
        /// 获取ApiTicket
        /// </summary>
        /// <returns>ApiTicket</returns>
        private string GetJsApiTicket()
        {
            var ticket = this._cache.GetCache(CACHE_TICKET_KEY);
            rui.logHelper.log("-------------4-------" + ticket);
            if (ticket != null)
                return ticket.ToString();
            try
            {
                string result = HttpGet(string.Format(URL_FORMAT_TICKET, this.GetAccessToken()));
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Dictionary<string, object> jsonObj = serializer.Deserialize<dynamic>(result);
                if (jsonObj.ContainsKey("ticket"))
                {
                    ticket = jsonObj["ticket"].ToString();
                    rui.logHelper.log("-------------5-------" + ticket);
                    this._cache.SetCache(CACHE_TICKET_KEY, ticket.ToString(), 7000);
                }
                else
                {
                    //为了程序正常运行，不抛出错误，可以记录日志
                    ticket = jsonObj["errmsg"];
                }
            }
            catch
            {
                //为了程序正常运行，不抛出错误，可以记录日志
                ticket = "there_is_an_error_when_getting_apiticket";
            }
            rui.logHelper.log("-------------6-------" + ticket);
            return ticket.ToString();
        }
        /// <summary>
        /// 获取jssdk签名配置对象
        /// </summary>
        /// <param name="jsapi">JsApiEnum,如:JsApiEnum.scanQRCode|JsApiEnum.onMenuShareQQ</param>
        /// <param name="clientUrl">签名是用了前端的路径</param>
        /// <returns>微信公众平台JsSdk的配置对象</returns>
        public SignPackage GetSignPackage(JsApiEnum jsapi, string clientUrl = "")
        {
            HttpContext httpcontext = System.Web.HttpContext.Current;
            string url = (!string.IsNullOrEmpty(httpcontext.Request.ServerVariables["HTTPS"])) && httpcontext.Request.ServerVariables["HTTPS"] != "off" ? "https://" : "http://";
            url += httpcontext.Request.ServerVariables["HTTP_HOST"];
            url += httpcontext.Request.ServerVariables["URL"];
            url += string.IsNullOrEmpty(httpcontext.Request.ServerVariables["QUERY_STRING"]) ? "" : httpcontext.Request.ServerVariables["QUERY_STRING"];
            return GetSignPackage(clientUrl, jsapi);
        }
        /// <summary>
        /// 获取jssdk签名配置对象
        /// </summary>
        /// <param name="url">当前页面url</param>
        /// <param name="jsapi">JsApiEnum,如:JsApiEnum.scanQRCode|JsApiEnum.onMenuShareQQ</param>
        /// <returns>微信公众平台JsSdk的配置对象</returns>
        public SignPackage GetSignPackage(string url, JsApiEnum jsapi)
        {

            /*
             * 签名字段
            noncestr=Wm3WZYTPz0wzccnW
            jsapi_ticket=sM4AOVdWfPE4DxkXGEs8VMCPGGVi4C3VM0P37wVUCFvkVAy_90u5h9nbSlYy3-Sl-HhTdfl2fzFy1AOcHKP7qg
            timestamp=1414587457
            url=http://mp.weixin.qq.com?params=value  
             */
            string noncestr = this.CreateNonceStr(16);
            string jsapi_tkcket = this.GetJsApiTicket();
            long timestamp = TimeStamp.Now();
            Dictionary<string, string> signData = new Dictionary<string, string>() { 
                {"noncestr",noncestr},
                {"jsapi_ticket",jsapi_tkcket},
                {"timestamp",timestamp.ToString()},
                {"url",url}
            };

            rui.logHelper.log($"jsapi_tkcket={jsapi_tkcket},noncestr={noncestr},timestamp={timestamp},url={url}");

            SignPackage result = new SignPackage()
            {
                appId = this.appId,
                timestamp = timestamp,
                nonceStr = noncestr,
                debug = this._debug,
                signature = new Signature().Sign(signData),
                jsApiList = jsapi.ToString().Replace(" ", "").Split(',')
            };
            return result;
        }
        /// <summary>
        /// 后台发起http请求
        /// </summary>
        /// <param name="url">请求URL</param>
        /// <returns>请求结果字符串</returns>
        private string HttpGet(string url)
        {
            return new WebClient().DownloadString(url);
        }

    }
}
