using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace rui.weixin.Message
{
    //消息处理接口
    public interface IHandle
    {
        //返回需要回复的内容
        string HandleMessage(XmlDocument xmldoc);
    }

    public class HanderFactory
    {
        //返回需要的消息处理器
        public static IHandle CreateHandler(XmlDocument xmldoc)
        {
            XmlNode MsgType = xmldoc.SelectSingleNode("/xml/MsgType");
            if (MsgType.InnerText == "event")
                return new Handle.EventHandle();
            else
                return new Handle.MessageHandle();
        }

        //获取接受信息的相关信息
        public static Dictionary<string, string> getXMLInfoToDic(XmlDocument xmldoc)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            foreach (XmlNode node in xmldoc.SelectSingleNode("/xml").ChildNodes)
            {
                dic.Add(node.Name, node.InnerText);
            }
            return dic;
        }
    }
}
