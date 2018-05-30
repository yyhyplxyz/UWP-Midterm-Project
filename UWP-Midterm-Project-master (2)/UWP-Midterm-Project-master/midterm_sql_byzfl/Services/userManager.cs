using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLitePCL;
using midterm_project.Models;
using System.Globalization;
using midterm_project;
using Windows.UI.Xaml.Media.Imaging;
using System.IO;
using Windows.Storage;
using Windows.Storage.Streams;
using System.Security.Cryptography;
using midterm_sql_byzfl.Services;

namespace midterm_project.Services
{
    public class userManager
    {
        public static void BuildDatabase()
        {
            if (((App)App.Current).conn == null)
            {
                ((App)App.Current).conn = new SQLiteConnection("SQLData.db");
            }
            //权限是0的时候为管理员，1的时候为服务员
            string sql = @"CREATE TABLE IF NOT EXISTS
                                UserDataBase (UserName   VARCHAR( 140 ) PRIMARY KEY NOT NULL,
                                            Password    VARCHAR( 140 ),
                                            Authority INT( 1 )
                                            );";
            using (var statement = ((App)App.Current).conn.Prepare(sql))
            {
                statement.Step();
            }
            if (!isExist("root"))
            {
                Insert(new userItem("root", "root", 0));
            }
        }
        //public userManager()
        //{
        //    BuildDatabase();
        //}
        public static bool Insert(userItem inputUserItem)
        {
            if (isExist(inputUserItem.UserName))
            {
                return false;
            }
            using (var custstmt = ((App)App.Current).conn.Prepare("INSERT INTO UserDataBase (UserName, Password, Authority) VALUES (?, ?, ?)"))
            {
                custstmt.Bind(1, inputUserItem.UserName);
                custstmt.Bind(2, safeManager.SHA1it(inputUserItem.Password));
                custstmt.Bind(3, inputUserItem.Authority);
                custstmt.Step();
            }
            return true;
        }

        //传递三个参数改动一条记录
        public static bool Update(string oldItemName, userItem newItem)
        {
            if (!isExist(oldItemName))
            {
                return false;
            }
            using (var custstmt = ((App)App.Current).conn.Prepare("UPDATE UserDataBase SET UserName = ? ,Password = ? ,Authority = ? WHERE UserName=?"))
            {
                custstmt.Bind(1, newItem.UserName);
                custstmt.Bind(2, safeManager.SHA1it(newItem.Password));
                custstmt.Bind(3, newItem.Authority);
                custstmt.Bind(4, oldItemName);
                custstmt.Step();
            }
            return true;
        }

        //通过名字删除一条记录
        public static bool Remove(string inputUserName)
        {
            if (!isExist(inputUserName))
            {
                return false;
            }
            using (var statement = ((App)App.Current).conn.Prepare("DELETE FROM UserDataBase WHERE UserName = ?"))
            {
                statement.Bind(1, inputUserName);
                statement.Step();
            }
            return true;
        }
        //得到所有记录
        public static List<userItem> GetAItem()
        {
            List<userItem> result = new List<userItem>();
            using (var statement = ((App)App.Current).conn.Prepare("SELECT UserName,Password,Authority FROM UserDataBase"))
            {
                while (SQLiteResult.ROW == statement.Step())
                {
                    userItem temp = new userItem(
                            (string)statement[0],
                            (string)statement[1],
                            (int)(Int64)statement[2]
                        );
                    result.Add(temp);
                }
            }
            return result;
        }

        //通过用户名查找人名
        public static userItem GetAItem(string inputUserName)
        {
            userItem result = null;
            using (var statement = ((App)App.Current).conn.Prepare("SELECT UserName,Password,Authority FROM UserDataBase WHERE UserName = ? "))
            {
                statement.Bind(1, inputUserName);
                while (SQLiteResult.ROW == statement.Step())
                {
                    result = new userItem(
                            (string)statement[0],
                            (string)statement[1],
                            (int)(Int64)statement[2]
                        );
                }
            }
            return result;
        }

        public static List<userItem> SearchName(string inputUserName)
        {
            List<userItem> result = new List<userItem>();
            string newInputUserName = "%" + inputUserName + "%";
            using (var statement = ((App)App.Current).conn.Prepare("SELECT UserName,Password,Authority FROM UserDataBase WHERE UserName LIKE ? "))
            {
                statement.Bind(1, newInputUserName);
                while (SQLiteResult.ROW == statement.Step())
                {
                    userItem temp = new userItem(
                            (string)statement[0],
                            (string)statement[1],
                            (int)(Int64)statement[2]
                        );
                    result.Add(temp);
                }
            }
            return result;
        }

        //通过权限查找人名
        public static List<userItem> queryByAuthority(string inputAuthority)
        {
            List<userItem> result = new List<userItem>();
            string newInputAuthority = "%" + inputAuthority + "%";
            using (var statement = ((App)App.Current).conn.Prepare("SELECT UserName,Password,Authority FROM UserDataBase WHERE Authority LIKE ? "))
            {
                statement.Bind(1, newInputAuthority);
                while (SQLiteResult.ROW == statement.Step())
                {
                    userItem temp = new userItem(
                            (string)statement[0],
                            (string)statement[1],
                            (int)(Int64)statement[2]
                        );
                    result.Add(temp);
                }
            }
            return result;
        }

        //判断一个人是否是管理员
        public static bool isAdministrator(string inputUserName)
        {
            using (var statement = ((App)App.Current).conn.Prepare("SELECT Authority FROM UserDataBase WHERE UserName = ? "))
            {
                statement.Bind(1, inputUserName);
                while (SQLiteResult.ROW == statement.Step())
                {
                    if ((int)(Int64)statement[0] == 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        //判断一个人是否存在
        public static bool isExist(string inputUserName)
        {
            try
            {
                using (var statement = ((App)App.Current).conn.Prepare("SELECT UserName FROM UserDataBase WHERE UserName = ? "))
                {
                    statement.Bind(1, inputUserName);
                    while (SQLiteResult.ROW == statement.Step())
                    {
                        if ((string)statement[0] == inputUserName)
                        {
                            return true;
                        }
                    }
                }
            }
            catch(Exception e)
            {
                return false;
            }
            return false;
        }

        public static void cleanDatabase()
        {
            try
            {
                using (var custstmt = ((App)App.Current).conn.Prepare("DELETE FROM UserDataBase"))
                {
                    custstmt.Step();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // 判断对应用户名和密码是否正确（或用户名不存在）
        public static bool check(string name, string password)
        {
            var find = GetAItem(name);
            if (find == null)
                return false;
            if (find.Password == safeManager.SHA1it(password))
                return true;
            return false;
        }
    }
}
