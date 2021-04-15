using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace rui.weixin.Message.Response
{
    /// <summary>
    /// 文本回复辅助类
    /// </summary>
    public class TextResponse : ResponseBase
    {
        protected string Content;

        public string responseXML = @"<xml>
                                    <ToUserName><![CDATA[{0}]]></ToUserName>
                                    <FromUserName><![CDATA[{1}]]></FromUserName>
                                    <CreateTime>{2}</CreateTime>
                                    <MsgType><![CDATA[{3}]]></MsgType>
                                    <Content><![CDATA[{4}]]></Content>
                                    </xml>";

        //构造函数
        public TextResponse(string ToUserName, string FromUserName, long CreateTime, string Content)
        {
            this.ToUserName = ToUserName;
            this.FromUserName = FromUserName;
            this.CreateTime = CreateTime;
            this.MsgType = "text";
            this.Content = Content;
        }

        //生成回复内容
        public override string getResponseText()
        {
            return string.Format(responseXML, this.ToUserName, this.FromUserName, this.CreateTime, this.MsgType, this.Content);
        }
    }

}
