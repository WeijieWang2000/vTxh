using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace rui.weixin.Toolkit
{
    //微信网站认证
    public class AuthHelper
    {
        /// <summary>
        /// <summary>
        /// 验证签名是否正确
        /// </summary>
        /// <param name="signature"></param>
        /// <param name="timestamp"></param>
        /// <param name="nonce"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static bool WebAuthenticate()
        {
            HttpContext content = HttpContext.Current;
            if (content.Request.HttpMethod.ToUpper() == "GET")
            {
                string signature = content.Request["signature"];
                string timestamp = content.Request["timestamp"];
                string nonce = content.Request["nonce"];
                string echostr = content.Request["echostr"];

                if (rui.weixin.Toolkit.AuthHelper.GetSignature(timestamp, nonce, WxConfig.TOKEN) == signature)
                {
                    rui.logHelper.log("success");
                    content.Response.Write(echostr);
                    content.Response.End();
                    return true;
                }
                else
                {
                    rui.logHelper.log("false");
                    content.Response.Write("验证错误");
                    content.Response.End();
                }
            }
            return false;
        }

        /// <summary>
        /// 返回正确的签名
        /// </summary>
        /// <param name="timestamp"></param>
        /// <param name="nonce"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        private static string GetSignature(string timestamp, string nonce, string token = WxConfig.TOKEN)
        {
            token = token ?? WxConfig.TOKEN;
            string[] arr = new[] { token, timestamp, nonce }.OrderBy(z => z).ToArray();
            string arrString = string.Join("", arr);
            System.Security.Cryptography.SHA1 sha1 = System.Security.Cryptography.SHA1.Create();
            byte[] sha1Arr = sha1.ComputeHash(Encoding.UTF8.GetBytes(arrString));
            StringBuilder enText = new StringBuilder();
            foreach (var b in sha1Arr)
            {
                enText.AppendFormat("{0:x2}", b);
            }
            return enText.ToString();
        }
    }
}
