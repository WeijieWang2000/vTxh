using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;

namespace rui.weixin.Message
{
    //微信消息接受和回复处理工具
    public class ReceiveMessage
    {
        //接受Post消息，并构建xmldoc对象，并对消息进行处理
        public static string HandleMessage()
        {
            try
            {
                HttpContext content = HttpContext.Current;
                if (content.Request.HttpMethod.ToUpper() == "POST")
                {
                    //获取接受到的消息体xmlDocument
                    XmlDocument xmldoc = rui.weixin.Message.ReceiveMessage.getXMLMessage();

                    //记录消息的类型
                    {
                        XmlNode MsgType = xmldoc.SelectSingleNode("/xml/MsgType");
                        rui.logHelper.log("消息类型:" + MsgType.InnerText);
                        if (MsgType.InnerText == "event")
                        {
                            XmlNode eventType = xmldoc.SelectSingleNode("/xml/Event");
                            rui.logHelper.log("事件类型:" + eventType.InnerText);
                        }
                    }
                    //根据请求的类型创建处理器并进行消息返回
                    {
                        IHandle handle = HanderFactory.CreateHandler(xmldoc);
                        string responseMessage = handle.HandleMessage(xmldoc);
                        return responseMessage;
                    }
                }
            }
            catch (Exception ex)
            {
                rui.logHelper.log(ex);
            }
            return "";
        }

        //把接受到的消息构建成xmldoc对象
        private static XmlDocument getXMLMessage()
        {
            HttpContext content = HttpContext.Current;
            string postString = "";
            Stream stream = content.Request.InputStream;
            Byte[] postBytes = new Byte[stream.Length];
            stream.Read(postBytes, 0, (Int32)stream.Length);
            postString = Encoding.UTF8.GetString(postBytes);

            //接受消息记录
            rui.logHelper.log(postString);

            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(new System.IO.MemoryStream(System.Text.Encoding.GetEncoding("UTF-8").GetBytes(postString)));

            return xmldoc;
        }
    }
}
