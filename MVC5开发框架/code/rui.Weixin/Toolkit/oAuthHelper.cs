using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace rui.weixin.Toolkit
{
    /// <summary>
    /// OAuth2的工具包  自动登录并获取用户信息
    /// </summary>
    public class OAuthHelper
    {
        //第一步，当未登录授权时，自动跳转到微信服务器用户授权页面，并设置回调页面
        public static void redirectOAuth2Url(string redirectUrl, string fromUrl)
        {
            string url = UrlHelper.get_OAuth_Authorize(redirectUrl, fromUrl);
            HttpContext.Current.Response.Redirect(url);
        }

        public static void redirectOAuth2UrlSimple(string redirectUrl, string fromUrl)
        {
            string url = UrlHelper.get_OAuth_AuthorizeSimple(redirectUrl, fromUrl);
            HttpContext.Current.Response.Redirect(url);
        }

        //第二步，内部方法，通过获取的code获取网页授权的accessToken
        private static string getAccessToken(string code)
        {
            string url = UrlHelper.get_OAuth_AccessToken(code);
            string jsonResult = HttpHelper.Get(url);
            return jsonResult;
        }

        //第三步，获取用户信息(复杂模式)
        public static weixinUserInfo getUserInfo(string code)
        {
            try
            {
                //第二步 通过Code获取access_token和openid
                string jsonResult = OAuthHelper.getAccessToken(code);
                rui.logHelper.log(jsonResult);

                //利用返回结果，获取access_token和openid
                JObject json = JObject.Parse(jsonResult);
                string access_token = json["access_token"].ToString();
                string openID = json["openid"].ToString();

                //复杂授权模式，允许继续获取用户的其它信息
                //使用access_token和openID获取用户信息
                string url = UrlHelper.get_User_UserinfoOAuth(access_token, openID);
                jsonResult = HttpHelper.Get(url);
                rui.logHelper.log("获取的用户信息:" + jsonResult);
                //解析用户json信息
                rui.weixin.Toolkit.weixinUserInfo userInfo
                    = UserInfoHelper.parseUserInfo(JObject.Parse(jsonResult), openID);

                //返回用户信息
                return userInfo;
            }
            catch (Exception ex)
            {
                rui.logHelper.log(ex);
            }
            return null;
        }

        //第三步，获取用户信息(简单模式)
        public static weixinUserInfo getUserInfoSimple(string code)
        {
            try
            {
                string jsonResult = OAuthHelper.getAccessToken(code);
                rui.logHelper.log(jsonResult);

                //利用返回结果，获取access_token和openid
                JObject json = JObject.Parse(jsonResult);
                string access_token = json["access_token"].ToString();
                string openID = json["openid"].ToString();

                //简单授权模式，只获取用户的openID
                rui.weixin.Toolkit.weixinUserInfo userInfo = new rui.weixin.Toolkit.weixinUserInfo();
                userInfo.openid = openID;

                return userInfo;
            }
            catch (Exception ex)
            {
                rui.logHelper.log(ex);
            }
            return null;
        }


        //获取用户的openID - 微信小程序使用
        public static string getOpenID(string code)
        {
            try
            {
                string url = $"https://api.weixin.qq.com/sns/jscode2session?appid={rui.weixin.WxConfig.APPID}&secret={rui.weixin.WxConfig.APPSECRET}&js_code={code}&grant_type=authorization_code";
                string jsonResult = HttpHelper.Get(url);

                rui.logHelper.log(jsonResult);

                //利用返回结果，获取access_token和openid
                JObject json = JObject.Parse(jsonResult);
                string openID = json["openid"].ToString();
                return openID;
            }
            catch (Exception ex)
            {
                rui.logHelper.log(ex);
            }
            return "";
        }
    }
}
