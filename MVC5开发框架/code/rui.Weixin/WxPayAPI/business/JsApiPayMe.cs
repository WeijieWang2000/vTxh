using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Runtime.Serialization;
using System.IO;
using System.Text;
using System.Net;
using System.Web.Security;
using LitJson;

namespace rui.weixin.WxPayAPI
{
    public class JsApiPayMe
    {
        //openid用于调用统一下单接口
        public string openid { get; set; }

        //订单编号
        public string orderCode { get; set; }

        //商品金额(单位分)，用于统一下单
        public int totalMoney { get; set; }

        //统一下单接口返回结果
        public WxPayData unifiedOrderResult { get; set; }

        //初始化，设置支付金额 === 上线前调整
        public JsApiPayMe(string openid,string orderCode, int totalMoney)
        {
            this.openid = openid;
            this.orderCode = orderCode;
            this.totalMoney = totalMoney;
        }

        /**
         * 调用统一下单，获得下单结果
         * @return 统一下单结果
         * @失败时抛异常WxPayException
         */
        public WxPayData GetUnifiedOrderResult(string trade_type= "JSAPI")
        {
            //统一下单
            WxPayData data = new WxPayData();
            data.SetValue("body", "善田江宁");
            data.SetValue("attach", "善田江宁");
            data.SetValue("out_trade_no", orderCode);
            data.SetValue("total_fee", totalMoney);
            data.SetValue("time_start", DateTime.Now.ToString("yyyyMMddHHmmss"));
            data.SetValue("time_expire", DateTime.Now.AddMinutes(10).ToString("yyyyMMddHHmmss"));
            data.SetValue("goods_tag", "test");
            data.SetValue("trade_type", trade_type);
            data.SetValue("openid", openid);

            //调用统一下单接口
            WxPayData result = WxPayApi.UnifiedOrder(data);
            if (result.GetValueStr("return_code") == "FAIL")
                rui.excptHelper.throwEx("return_msg");

            if (!result.IsSet("appid") || !result.IsSet("prepay_id") || result.GetValue("prepay_id").ToString() == "")
            {
                Log.Error(this.GetType().ToString(), result.GetValue("err_code_des").ToString());
                throw new WxPayException(result.GetValue("err_code_des").ToString());
            }
            unifiedOrderResult = result;
            return result;
        }

        /**
        *  
        * 从统一下单成功返回的数据中获取微信浏览器调起jsapi支付所需的参数，
        * 微信浏览器调起JSAPI时的输入参数格式如下：
        * {
        *   "appId" : "wx2421b1c4370ec43b",     //公众号名称，由商户传入     
        *   "timeStamp":" 1395712654",         //时间戳，自1970年以来的秒数     
        *   "nonceStr" : "e61463f8efa94090b1f366cccfbbb444", //随机串     
        *   "package" : "prepay_id=u802345jgfjsdfgsdg888",     
        *   "signType" : "MD5",         //微信签名方式:    
        *   "paySign" : "70EA570631E4BB79628FBCA90534C63FF7FADD89" //微信签名 
        * }
        * @return string 微信浏览器调起JSAPI时的输入参数，json格式可以直接做参数用
        * 更详细的说明请参考网页端调起支付API：http://pay.weixin.qq.com/wiki/doc/api/jsapi.php?chapter=7_7
        */
        public string GetJsApiParameters()
        {
            Log.Debug(this.GetType().ToString(), "JsApiPay::GetJsApiParam is processing...");

            WxPayData jsApiParam = new WxPayData();
            jsApiParam.SetValue("appId", unifiedOrderResult.GetValue("appid"));
            jsApiParam.SetValue("timeStamp", WxPayApi.GenerateTimeStamp());
            jsApiParam.SetValue("nonceStr", WxPayApi.GenerateNonceStr());
            jsApiParam.SetValue("package", "prepay_id=" + unifiedOrderResult.GetValue("prepay_id"));
            jsApiParam.SetValue("signType", "MD5");
            jsApiParam.SetValue("paySign", jsApiParam.MakeSign());

            string parameters = jsApiParam.ToJson();

            Log.Debug(this.GetType().ToString(), "Get jsApiParam : " + parameters);
            return parameters;
        }
    }
}