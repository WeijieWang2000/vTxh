using System;
using System.Collections.Generic;
using System.Web;

namespace rui.weixin.WxPayAPI
{
    /**
    * 	配置账号信息
    */
    public class WxPayConfig
    {
        //=======【基本信息设置】 五个数据必须根据客户环境配置 =====================================
        /* 微信公众号信息配置
        * APPID：绑定支付的APPID（小程序或者微信公众号的）
        * APPSECRET：公众帐号secert（小程序或者微信公众号的,仅JSAPI支付的时候需要配置）
        * MCHID：商户号（微信支付开通后，分配的商户号）
        * KEY：商户支付密钥，参考开户邮件设置（必须配置），
        * IP:商户系统后台机器IP
        */
        public const string APPID = WxConfig.APPID;
        public const string APPSECRET = WxConfig.APPSECRET;
        public const string IP = WxConfig.IP;
        public const string MCHID = "1502906421";
        public const string KEY = "0fcb4e1731a13c781651cbb699191c56";

        //=======【证书路径设置】===================================== 
        /* 证书路径,注意应该填写绝对路径（仅退款、撤销订单时需要）
        */
        public const string SSLCERT_PATH = "/cert/apiclient_cert.p12";
        public const string SSLCERT_PASSWORD = "1502906421";

        //=======【支付结果通知url】===================================== 
        /* 支付结果通知回调url，用于商户接收支付结果
        */
        public const string NOTIFY_URL = WxConfig.DomainName + "/Home/weixinPayCallBack";

        //=======【上报信息配置】===================================
        /* 测速上报等级，0.关闭上报; 1.仅错误时上报; 2.全量上报
        */
        public const int REPORT_LEVENL = 1;

        //=======【日志级别】===================================
        /* 日志等级，0.不输出日志；1.只输出错误信息; 2.输出错误和正常信息; 3.输出错误信息、正常信息和调试信息
        */
        public const int LOG_LEVENL = 3;
    }
}