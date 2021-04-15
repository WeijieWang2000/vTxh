using rui.weixin.Message.Response;
using rui.weixin.Toolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace rui.weixin.Message.Handle
{
    //事件消息方法处理
    class EventHandle : IHandle
    {
        //根据事件类型调用各种方法
        public string HandleMessage(System.Xml.XmlDocument xmldoc)
        {
            XmlNode EventType = xmldoc.SelectSingleNode("/xml/Event");
            switch (EventType.InnerText)
            {
                case "subscribe":
                    return this.subscribe(xmldoc);
                case "unsubscribe":
                    return this.unsubscribe(xmldoc);
            }
            return "";
        }

        //订阅事件返回
        private string subscribe(XmlDocument xmldoc)
        {
            Dictionary<string, string> xml = HanderFactory.getXMLInfoToDic(xmldoc);
            string ToUserName = xml["FromUserName"];
            rui.logHelper.log("关注事件:" + ToUserName);

            //获取关注用户信息
            weixinUserInfo user = rui.weixin.Toolkit.UserInfoHelper.getUserInfo(ToUserName);
            rui.weixin.Toolkit.UserInfoHelper.insertUser(user,true);

            string FromUserName = xml["ToUserName"];
            string responseContent = "欢迎光临sju在线知识库";
            TextResponse response = new TextResponse(ToUserName, FromUserName, DateTime.Now.Ticks, responseContent);
            return response.getResponseText();
        }

        //退订事件返回
        private string unsubscribe(XmlDocument xmldoc)
        {
            Dictionary<string, string> xml = HanderFactory.getXMLInfoToDic(xmldoc);
            string ToUserName = xml["FromUserName"];
            rui.logHelper.log("取消关注事件:" + ToUserName);

            //设置用户取消关注
            rui.weixin.Toolkit.UserInfoHelper.cancelAttentive(ToUserName);

            string FromUserName = xml["ToUserName"];
            string responseContent = "欢迎下次再来";
            TextResponse response = new TextResponse(ToUserName, FromUserName, DateTime.Now.Ticks, responseContent);
            return response.getResponseText();
        }
    }
}
