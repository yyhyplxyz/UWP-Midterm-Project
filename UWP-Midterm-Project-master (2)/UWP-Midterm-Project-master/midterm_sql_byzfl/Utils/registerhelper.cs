using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace midterm_sql_byzfl.Utils
{
    class registerhelper
    {
        public static string singleSend(string mobile1, string randnum, string last = "【UWP项目】")
        {
            string apikey = "**********************"; //配置您申请的appkey  
                                                      // 发送的手机号  
            string mobile = mobile1;
            // 发送内容  
            string text = "【元昊提醒您】您的验证码是" + randnum + "";
            // 智能模板发送短信url  
            string url_send_sms = "https://sms.yunpian.com/v2/sms/single_send.json";
            string data_send_sms = "apikey=" + apikey + "&mobile=" + mobile + "&text=" +
                text;
            return HttpPost(url_send_sms, data_send_sms);
        }
        public static string HttpPost(string Url, string postDataStr)
        {
            byte[] dataArray = Encoding.UTF8.GetBytes(postDataStr);
            // Console.Write(Encoding.UTF8.GetString(dataArray));  

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = dataArray.Length;
            //request.CookieContainer = cookie;  
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(dataArray, 0, dataArray.Length);
            dataStream.Close();
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader reader =
                    new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                String res = reader.ReadToEnd();
                reader.Close();
                //Console.Write("\nResponse Content:\n" + res + "\n");  
                return res;
            }
            catch (Exception e)
            {
                return e.Message + e.ToString();
            }
        }
        public static string GetRandomString(int iLength)
        {
            string buffer = "0123456789";// 随机字符中也可以为汉字（任何）  
            StringBuilder sb = new StringBuilder();
            Random r = new Random();
            int range = buffer.Length;
            for (int i = 0; i < iLength; i++)
            {
                sb.Append(buffer.Substring(r.Next(range), 1));
            }
            return sb.ToString();
        }
    }
}
