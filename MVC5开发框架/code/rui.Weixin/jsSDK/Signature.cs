using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace rui.weixin.jsSDK
{
    /// <summary>
    /// 签名类
    /// </summary>
    public class Signature
    {
        /// <summary>
        /// 生成签名
        /// </summary>
        /// <param name="data">参与签名的参数字典</param>
        /// <returns>签名</returns>
        public string Sign(Dictionary<string, string> data)
        {
            var dataList = data.ToList();

            dataList.Sort(ParameterKeyComparison);
            var queryString = dataList.Aggregate(string.Empty, (query, item) => string.Concat(query, "&", item.Key, "=", item.Value)).TrimStart('&');

            rui.logHelper.log("------签名串-----" + queryString);

            string result = "";
            using (var sha1 = new SHA1CryptoServiceProvider())
            {
                var hashed = sha1.ComputeHash(Encoding.Default.GetBytes(queryString));
                result = HexStringFromBytes(hashed);
            }
            rui.logHelper.log("------签名后-----" + result);
            return result;

        }
        static string HexStringFromBytes(byte[] bytes)
        {
            var sb = new StringBuilder();
            foreach (byte b in bytes)
            {
                var hex = b.ToString("x2");
                sb.Append(hex);
            }
            return sb.ToString();
        }

        static int ParameterKeyComparison(KeyValuePair<string, string> x, KeyValuePair<string, string> y)
        {
            return x.Key.CompareTo(y.Key);
        }
    }
}
