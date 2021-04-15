using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rui.weixin.Toolkit
{
    /// <summary>
    /// 微信用户辅助类
    /// </summary>
    public class UserInfoHelper
    {
        /// <summary>
        /// 用户新增
        /// 如果以前有，则更新，否则新增
        /// 来源关注,则更新性别，头像，昵称
        /// 不来源关注，只保存openID
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <param name="isFromAttentive">是否来自关注</param>
        public static void insertUser(weixinUserInfo user, bool isFromAttentive)
        {
            if (user.sex == "1")
                user.sex = "男";
            if (user.sex == "2")
                user.sex = "女";
            if (user.sex == "0")
                user.sex = "密";

            using (rui.dbHelper helper = rui.dbHelper.createHelper())
            {
                string sql = " SELECT * FROM dbo.wx_User WHERE weixinUserCode=@userCode ";
                bool isExist = helper.ExecuteExist(sql, new { userCode = user.openid });  
                if(isExist == true)
                {
                    //已存在，来源关注更新用户信息
                    if (isFromAttentive == true)
                    {
                        sql = " UPDATE dbo.wx_User SET isAttentive='是', nickName=@nickName, userSex=@userSex,headerImg=@headerImg WHERE weixinUserCode=@weixinUserCode ";
                        helper.Execute(sql, new { weixinUserCode = user.openid, nickName = user.nickname, userSex = user.sex, headerImg = user.headimgurl });
                    }
                }
                else
                {
                    //不存在，来源关注新增用户信息
                    if (isFromAttentive == true)
                    {
                        sql = @" INSERT INTO dbo.wx_User(weixinUserCode,nickName,userSex,headerImg,isAttentive,sourceFrom,importDate)
                            VALUES (@weixinUserCode,@nickName,@userSex,@headerImg,'是','关注',GETDATE()) ";
                        helper.Execute(sql, new { weixinUserCode = user.openid, nickName = user.nickname, userSex = user.sex, headerImg = user.headimgurl });
                    }
                    else
                    {
                        //未关注来访用户,只保存OpenID
                        sql = @" INSERT INTO dbo.wx_User(weixinUserCode,isAttentive,sourceFrom,importDate) 
                            VALUES (@weixinUserCode,'否','访问',GETDATE()) ";
                        helper.Execute(sql, new { weixinUserCode = user.openid, nickName = user.nickname, userSex = user.sex, headerImg = user.headimgurl });
                    }
                }
            }
        }

        /// <summary>
        /// 用户取消关注
        /// 设置用户取消关注
        /// </summary>
        /// <param name="userCode"></param>
        public static void cancelAttentive(string userCode)
        {
            using (rui.dbHelper helper = rui.dbHelper.createHelper())
            {
                string sql = @" UPDATE dbo.prj_User SET isAttentive='否',isCert='否',certDate=null,isSubmit='否',isAudit='否',userCodeAudit=null,auditDate=null WHERE weixinUserCode=@userCode ";
                helper.Execute(sql, new { userCode });
            }
        }

        /// <summary>
        /// 通过openID获取用户其它信息
        /// 1)获取accessToken
        /// 2)获取用户信息
        /// </summary>
        /// <returns></returns>
        public static weixinUserInfo getUserInfo(string openID)
        {
            AccessTokenHelper tokenHelper = AccessTokenHelper.getInsance();
            string access_token = tokenHelper.getToken();

            //使用access_token和openID获取用户信息
            string url = UrlHelper.get_User_Userinfo(access_token, openID);
            string jsonResult = HttpHelper.Get(url);
            rui.logHelper.log("获取的用户信息:" + jsonResult);
            JObject json = JObject.Parse(jsonResult);

            //返回用户信息
            weixinUserInfo user = UserInfoHelper.parseUserInfo(json, openID);
            return user;
        }

        /// <summary>
        /// 更新关注用户信息的信息
        /// </summary>
        /// <returns></returns>
        public static void updateAttentiveInfo()
        {
            AccessTokenHelper tokenHelper = AccessTokenHelper.getInsance();
            string access_token = tokenHelper.getToken();

            string url = UrlHelper.get_User_UserList();
            string jsonResult = HttpHelper.Get(url);
            rui.logHelper.log("获取关注用户列表:" + jsonResult);
            JObject json = JObject.Parse(jsonResult);
            string openIDList = json["data"]["openid"].ToString();

            List<string> list = rui.dbTools.getList(openIDList.Replace("[", "").Replace("]", "").Replace("\"", ""));
            foreach (var openID in list)
            {
                weixinUserInfo userInfo = getUserInfo(openID.Trim());
                rui.weixin.Toolkit.UserInfoHelper.insertUser(userInfo, true);
            }
        }

        /// <summary>
        /// 解析返回的用户json数据
        /// </summary>
        /// <param name="json"></param>
        /// <param name="openID"></param>
        /// <returns></returns>
        public static weixinUserInfo parseUserInfo(JObject json, string openID)
        {
            weixinUserInfo user = new weixinUserInfo();
            user.openid = openID;

            try
            {
                user.nickname = json["nickname"].ToString();
                user.sex = json["sex"].ToString();
                user.headimgurl = json["headimgurl"].ToString();
                user.language = json["language"].ToString();
                user.city = json["city"].ToString();
                user.province = json["province"].ToString();
                user.country = json["country"].ToString();
                user.privilege = json["privilege"].ToString();
            }
            catch (Exception ex)
            {
                rui.logHelper.log("用户信息不全");
            }

            return user;
        }
    }


    /// <summary>
    /// 微信所获取的用户信息模型
    /// </summary>
    public class weixinUserInfo
    {
        public string openid;
        public string nickname;
        public string userName;
        public string telphone;
        public string sex;
        public string language;
        public string city;
        public string province;
        public string country;
        public string headimgurl;
        public string privilege;
    }


}
