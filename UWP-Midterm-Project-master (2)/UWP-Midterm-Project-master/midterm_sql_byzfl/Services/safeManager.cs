using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace midterm_sql_byzfl.Services
{
    class safeManager
    {
        /// <summary>  
        /// SHA1 加密  
        /// </summary>  
        public static string SHA1it(string content)
        {
            return SHA1(content, Encoding.UTF8);
        }

        public static string SHA1(string content, Encoding encode)
        {
            try
            {
                SHA1 sha1 = new SHA1CryptoServiceProvider();
                byte[] bytes_in = encode.GetBytes(content);
                byte[] bytes_out = sha1.ComputeHash(bytes_in);
                sha1.Dispose();
                string result = BitConverter.ToString(bytes_out);
                result = result.Replace("-", "");
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("SHA1加密出错：" + ex.Message);
            }
        }

        /// <summary>  
        /// 防脱裤函数 
        /// </summary> 

        public static bool checkInjection(string arr)
        {
            string blockList = "exec |insert |select |delete |update |count |chr |mid |master |truncate |declare " +
                "|exec|insert|select|delete|update|count|chr|mid|master|truncate|declare" + 
                "|*|\'|\\|--|/*|*/";
            //try
            //{
                if(arr != null && arr != "")
                {
                    string[] bl = blockList.Split('|');
                    for(int i = 0; i < bl.Count(); i++)
                    {
                        if(arr.IndexOf(bl[i]) >= 0)
                        {
                            return false;
                        }
                    }
                }
            //}
            //catch(Exception e)
            //{
            //    return false;
            //}
            return true;
        }
    }
}
