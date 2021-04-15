using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace rui.weixin.Toolkit
{
    //待测试
    public class MediaHelper
    {
        /// <SUMMARY> 
        /// 上传临时多媒体文件,返回 MediaId 
        /// </SUMMARY> 
        /// <PARAM name="ACCESS_TOKEN"></PARAM> 
        /// <PARAM name="Type"></PARAM> 
        /// <PARAM name="filePath">上传文件的站内路径</PARAM> 
        /// <RETURNS>文件的服务器ID</RETURNS> 
        public string UploadMultimedia(string ACCESS_TOKEN, string Type,string filePath)
        {
            string result = "";
            string wxUrl = UrlHelper.get_Media_upload(ACCESS_TOKEN, Type);
            filePath = HttpContext.Current.Server.MapPath(filePath);
            WebClient webClient = new WebClient();
            webClient.Credentials = CredentialCache.DefaultCredentials;
            try
            {
                byte[] responseArray = webClient.UploadFile(wxUrl, "POST", filePath);
                string jsonResult = System.Text.Encoding.Default.GetString(responseArray, 0, responseArray.Length);

                //处理返回结果，提取media_id
                JObject json = JObject.Parse(jsonResult);
                result = json["media_id"].ToString();
            }
            catch (Exception ex)
            {
                result = "Error:" + ex.Message;
            }
            return result;
        }

        /// <SUMMARY> 
        /// 下载保存多媒体文件,返回多媒体保存路径 
        /// </SUMMARY> 
        /// <PARAM name="ACCESS_TOKEN"></PARAM> 
        /// <PARAM name="MEDIA_ID">文件的服务器ID</PARAM> 
        /// <PARAM name="savePath">服务器的保存位置</PARAM> 
        /// <RETURNS>是否成功</RETURNS> 
        public bool DownloadMultimedia(string ACCESS_TOKEN, string MEDIA_ID,string savePath)
        {
            string file = string.Empty;
            string content = string.Empty;
            string strPath = string.Empty;
            string wxUrl = UrlHelper.get_Media_get(ACCESS_TOKEN, MEDIA_ID);

            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(wxUrl);
            req.Method = "GET";
            bool result = false;
            using (WebResponse wr = req.GetResponse())
            {
                strPath = wr.ResponseUri.ToString();
                WebClient webClient = new WebClient();
                savePath = HttpContext.Current.Server.MapPath(savePath);
                try
                {
                    rui.logHelper.log("1");
                    Stream stream = wr.GetResponseStream();
                    Image img = Image.FromStream(stream);
                    MediaHelper.ImgAutoRotate(img);
                    img.Save(savePath);
                    img.Dispose();
                    stream.Dispose();
                    rui.logHelper.log("2");
                    //webClient.DownloadFile(strPath, savePath);
                    //webClient.Dispose();
                    //wr.Close();

                    result = true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            req.Abort();
            GC.Collect();
            return result;
        }

        /// <summary>
        /// EXIF 内部 Orientation参数的值进行图片的旋转
        /// </summary>
        /// <param name="img"></param>
        public static void ImgAutoRotate(Image img)
        {
            try
            {
                rui.logHelper.log("3");
                var result = from a in img.PropertyItems
                             where a.Id == 0x0112
                             select a.Value;
                if (result != null && result.Count() > 0)
                {
                    byte[] buffer = (byte[])result.First();
                    rotating(img, buffer[0]);
                }
            }
            catch (Exception ex)
            {
                rui.logHelper.log(ex);
            }
        }

        private static void rotating(Image img, int orien)
        {
            rui.logHelper.log("图片角度" + orien.ToString());
            switch (orien)
            {
                case 2:
                    img.RotateFlip(RotateFlipType.RotateNoneFlipX);//horizontal flip
                    break;
                case 3:
                    img.RotateFlip(RotateFlipType.Rotate180FlipNone);//right-top
                    break;
                case 4:
                    img.RotateFlip(RotateFlipType.RotateNoneFlipY);//vertical flip
                    break;
                case 5:
                    img.RotateFlip(RotateFlipType.Rotate90FlipX);
                    break;
                case 6:
                    img.RotateFlip(RotateFlipType.Rotate90FlipNone);//right-top
                    break;
                case 7:
                    img.RotateFlip(RotateFlipType.Rotate270FlipX);
                    break;
                case 8:
                    img.RotateFlip(RotateFlipType.Rotate270FlipNone);//left-bottom
                    break;
                default:
                    break;
            }
        }
    }
}
