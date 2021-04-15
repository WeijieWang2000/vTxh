using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using rui.weixin.Toolkit;
using System.Data;
using Newtonsoft.Json;

namespace rui.weixin.Message
{
    public class TempleteMessage
    {
        public static string send(string json)
        {
            string value = HttpHelper.Post(UrlHelper.get_Template_Send(), json);
            return value;
        }

        //参考例子
        //在网站根目录下创建一个json目录，把模板的json放到里边，并设定替换符号
        //获取到文件内容后，进行内容替换，替换后，进行发送
        public static void send活动报名成功提醒()
        {
            var msgData = new
            {
                touser = "接收者",
                template_id = "9LikNtHM_H3N8K6XG8XetaNL3bB2Zom5UIsGVRLybFU",
                url = "关联网址",
                data = new
                {
                    first = new { value = "数据值", color = "#173177" },
                    keyword1 = new { value = "数据值", color = "#173177" },
                    keyword2 = new { value = "数据值", color = "#173177" },
                    keyword3 = new { value = "数据值", color = "#173177" },
                    keyword4 = new { value = "数据值", color = "#173177" },
                    remark = new { value = "数据值", color = "#173177" }
                }
            };
            string json = JsonConvert.SerializeObject(msgData);
            string value = TempleteMessage.send(json);
        }

        /// <summary>
        /// 用户商品下单提醒
        /// </summary>
        public static void send下单成功提醒(string weixinID,string productName,int orderQty,string totalMoney,DateTime orderDate,string receiveAddress)
        {
            var msgData = new
            {
                touser = weixinID,
                template_id = "77O5yc9aAyJTDGT5w5OtAfFmdbSimB5bgiE8MMfUweg",
                //url = "关联网址",
                data = new
                {
                    first = new { value ="商品名称："+ productName, color = "#173177" },
                    keyword1 = new { value = orderQty, color = "#173177" },
                    keyword2 = new { value = totalMoney, color = "#173177" },
                    keyword3 = new { value = "微信支付", color = "#173177" },
                    keyword4 = new { value = orderDate, color = "#173177" },
                    keyword5 = new { value = receiveAddress, color = "#173177" },
                    remark = new { value = "来新订单啦，请及时查看新订单！", color = "#173177" }
                }
            };
            string json = JsonConvert.SerializeObject(msgData);
            string value = TempleteMessage.send(json);
        }

        /// <summary>
        /// 申请退款提醒
        /// </summary>
        /// <param name="weixinID"></param>
        /// <param name="productName"></param>
        /// <param name="orderQty"></param>
        /// <param name="orderDate"></param>
        /// <param name="receiveAddress"></param>
        public static void send申请退款提醒(string weixinID, string CustomerName, string orderCode, string totalMoney,string returnTotalMoney,DateTime handleDate)
        {
            var msgData = new
            {
                touser = weixinID,
                template_id = "4GM_ZMnB6i5IOZFOWLU8XsKJOoeZN012l55JCySD6zM",
                //url = "关联网址",
                data = new
                {
                    first = new { value = "用户" + CustomerName+"申请退款", color = "#173177" },
                    keyword1 = new { value = orderCode, color = "#173177" },
                    //keyword2 = new { value = "数据值", color = "#173177" },
                    keyword3 = new { value = totalMoney, color = "#173177" },
                    keyword4 = new { value = returnTotalMoney, color = "#173177" },
                    keyword5 = new { value = handleDate, color = "#173177" },
                    remark = new { value = "用户申请退款，请及时处理！", color = "#173177" }
                }
            };
            string json = JsonConvert.SerializeObject(msgData);
            string value = TempleteMessage.send(json);
        }

        public static void send申请退货提醒(string weixinID, string CustomerName, string orderCode,string productName,string returnTotalMoney)
        {
            var msgData = new
            {
                touser = weixinID,
                template_id = "ADo9D9YSAEAPdx6dkOH3bTh5miNY3B49d3kd3UzLLvI",
                //url = "关联网址",
                data = new
                {
                    first = new { value = "用户" + CustomerName + "申请退货", color = "#173177" },
                    keyword1 = new { value = orderCode, color = "#173177" },
                    keyword2 = new { value = productName, color = "#173177" },
                    keyword3 = new { value = returnTotalMoney, color = "#173177" },
                    remark = new { value = "用户申请退货，请及时处理！", color = "#173177" }
                }
            };
            string json = JsonConvert.SerializeObject(msgData);
            string value = TempleteMessage.send(json);
        }

    }
}
