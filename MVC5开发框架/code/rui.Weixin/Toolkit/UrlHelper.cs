using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace rui.weixin.Toolkit
{
    //构造要请求的接口地址
    public class UrlHelper
    {
        #region 用户菜单
        public static string get_Menu_Create()
        {
            string access_token = AccessTokenHelper.getInsance().getToken();
            string url = $"https://api.weixin.qq.com/cgi-bin/menu/create?access_token={access_token}";
            return url;
        }  
        #endregion


        #region 模板消息
        public static string get_Template_Send()
        {
            string access_token = AccessTokenHelper.getInsance().getToken();
            string url = $"https://api.weixin.qq.com/cgi-bin/message/template/send?access_token={access_token}";
            return url;
        }
        #endregion

        #region 用户管理
        public static string get_User_UserList()
        {
            string access_token = AccessTokenHelper.getInsance().getToken();
            string url = $"https://api.weixin.qq.com/cgi-bin/user/get?access_token={access_token}";
            return url;
        }

        //获取用户信息 UserInfo OAuth2 授权过程使用
        public static string get_User_UserinfoOAuth(string accessToken, string openID)
        {
            string url = $"https://api.weixin.qq.com/sns/userinfo?access_token={accessToken}&openid={openID}&lang=zh_CN";
            return url;
        }

        //获取用户信息 UserInfo 非授权过程使用
        public static string get_User_Userinfo(string accessToken, string openID)
        {
            string url = $"https://api.weixin.qq.com/cgi-bin/user/info?access_token={accessToken}&openid={openID}&lang=zh_CN";
            return url;
        }
        #endregion


        #region Auth2授权
        //认证
        public static string get_OAuth_Authorize(string oAuth2Path,string fromUrl)
        {
            string appID = WxConfig.APPID;
            string redirectUrl = WxConfig.DomainName + oAuth2Path;
            string url = string.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_userinfo&state={2}#wechat_redirect", appID, redirectUrl, fromUrl);
            return url;
        }
        //认证 - 简单模式
        public static string get_OAuth_AuthorizeSimple(string oAuth2Path, string fromUrl)
        {
            string appID = WxConfig.APPID;
            string redirectUrl = WxConfig.DomainName + oAuth2Path;
            string url = string.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_base&state={2}#wechat_redirect", appID, redirectUrl, fromUrl);
            return url;
        }
        //获取accessToken
        public static string get_OAuth_AccessToken(string Code)
        {
            string appID = WxConfig.APPID;
            string appSecret = WxConfig.APPSECRET;
            string url = string.Format("https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code", appID, appSecret, Code);
            return url;
        }
        #endregion

        #region 多媒体文件下载
        public static string get_Media_get(string accessToken, string mediaID)
        {
            string url = string.Format("http://file.api.weixin.qq.com/cgi-bin/media/get?access_token={0}&media_id={1}", accessToken, mediaID);
            return url;
        }
        public static string get_Media_upload(string accessToken, string mediaType)
        {
            string url = string.Format("http://file.api.weixin.qq.com/cgi-bin/media/upload?access_token={0}&type={1}", accessToken, mediaType);
            return url;
        }
        #endregion
    }
}
