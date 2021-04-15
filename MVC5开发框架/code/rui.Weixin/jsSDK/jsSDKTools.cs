using rui.weixin.jsSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rui.weixin.jsSDK
{
    public class jsSDKTools
    {
        /// <summary>
        /// 获取jsConfig配置数据
        /// </summary>
        /// <returns></returns>
        public static string getJSConfig()
        {
            string appId = rui.weixin.WxConfig.APPID;
            string appSecret = rui.weixin.WxConfig.APPSECRET;
            bool debug = false;
            JSSDK sdk = new JSSDK(appId, appSecret, debug);
            SignPackage config = sdk.GetSignPackage
                (JsApiEnum.getLocation | JsApiEnum.openLocation);
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            string jsConfig = serializer.Serialize(config);
            return jsConfig;
        }
    }
}
