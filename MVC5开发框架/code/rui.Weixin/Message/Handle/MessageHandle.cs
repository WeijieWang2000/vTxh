using rui.weixin.Message.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace rui.weixin.Message.Handle
{
    //文本消息方法
    public class MessageHandle : IHandle
    {
        //根据消息类型调用各种方法
        public string HandleMessage(XmlDocument xmldoc)
        {
            XmlNode EventType = xmldoc.SelectSingleNode("/xml/MsgType");
            switch (EventType.InnerText)
            {
                case "text":
                    return this.text(xmldoc);
                case "image":
                    return this.image(xmldoc);
            }
            return "";
        }

        //文本消息返回
        private string text(XmlDocument xmldoc)
        {
            Dictionary<string, string> xml = HanderFactory.getXMLInfoToDic(xmldoc);
            string ToUserName = xml["FromUserName"];
            string FromUserName = xml["ToUserName"];
            string Content = xml["Content"];
            string responseContent = string.Format("你发送的是{0}", Content);
            TextResponse response = new TextResponse(ToUserName, FromUserName, DateTime.Now.Ticks, responseContent);
            return response.getResponseText();
        }

        //图片消息返回
        private string image(XmlDocument xmldoc)
        {
            return "";
        }
    }
}
