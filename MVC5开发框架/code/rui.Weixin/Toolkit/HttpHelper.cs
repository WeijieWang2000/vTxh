using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;

namespace rui.weixin.Toolkit
{
    public class HttpHelper
    {
        public static string Get(string url)
        {
            System.GC.Collect();
            string result = "";

            HttpWebRequest request = null;
            HttpWebResponse response = null;

            //请求url以获取数据
            try
            {
                //设置最大连接数
                ServicePointManager.DefaultConnectionLimit = 200;
                //设置https验证方式
                if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                {
                    ServicePointManager.ServerCertificateValidationCallback =
                            new RemoteCertificateValidationCallback(CheckValidationResult);
                }

                request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";

                //获取服务器返回
                response = (HttpWebResponse)request.GetResponse();

                //获取HTTP返回数据
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                result = sr.ReadToEnd().Trim();
                sr.Close();
            }
            catch (System.Threading.ThreadAbortException ex)
            {
                rui.logHelper.log(ex);
                System.Threading.Thread.ResetAbort();
                throw ex;
            }
            catch (WebException ex)
            {
                rui.logHelper.log(ex);
                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    rui.logHelper.log("StatusCode : " + ((HttpWebResponse)ex.Response).StatusCode);
                    rui.logHelper.log("StatusDescription : " + ((HttpWebResponse)ex.Response).StatusDescription);
                }
                throw ex;
            }
            catch (Exception ex)
            {
                rui.logHelper.log(ex);
                throw ex;
            }
            finally
            {
                //关闭连接和流
                if (response != null)
                {
                    response.Close();
                }
                if (request != null)
                {
                    request.Abort();
                }
            }
            return result;
        }

        public static string Post(string url, string postData)
        {
            System.GC.Collect();//垃圾回收，回收没有正常关闭的http连接

            string result = "";//返回结果

            HttpWebRequest request = null;
            HttpWebResponse response = null;
            Stream reqStream = null;

            try
            {
                //设置最大连接数
                ServicePointManager.DefaultConnectionLimit = 200;
                //设置https验证方式
                if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                {
                    ServicePointManager.ServerCertificateValidationCallback =
                            new RemoteCertificateValidationCallback(CheckValidationResult);
                }

                request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";

                //设置POST的数据类型和长度
                request.ContentType = "text/xml";
                byte[] data = System.Text.Encoding.UTF8.GetBytes(postData);
                request.ContentLength = data.Length;

                //往服务器写入数据
                reqStream = request.GetRequestStream();
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();

                //获取服务端返回
                response = (HttpWebResponse)request.GetResponse();

                //获取服务端返回数据
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                result = sr.ReadToEnd().Trim();
                sr.Close();
            }
            catch (System.Threading.ThreadAbortException ex)
            {
                rui.logHelper.log(ex);
                System.Threading.Thread.ResetAbort();
                throw ex;
            }
            catch (WebException ex)
            {
                rui.logHelper.log(ex);
                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    rui.logHelper.log("StatusCode : " + ((HttpWebResponse)ex.Response).StatusCode);
                    rui.logHelper.log("StatusDescription : " + ((HttpWebResponse)ex.Response).StatusDescription);
                }
                throw ex;
            }
            catch (Exception ex)
            {
                rui.logHelper.log(ex);
                throw ex;
            }
            finally
            {
                //关闭连接和流
                if (response != null)
                {
                    response.Close();
                }
                if (request != null)
                {
                    request.Abort();
                }
            }
            return result;
        }

        //证书验证
        public static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            //直接确认，否则打不开    
            return true;
        }
    }
}
