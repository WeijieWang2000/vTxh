using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace rui.weixin.Message
{
    //消息回复接口
    public class ResponseBase
    {
        protected string ToUserName;
        protected string FromUserName;
        protected long CreateTime;
        protected string MsgType;

        //获取回应文本
        public virtual string getResponseText() { return null;  }
    }
}
