using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.AccessCache;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
using midterm_project.Models;

namespace midterm_project.Services
{   
    //材料数据库管理类
    class materialManager
    {
        //错误码，分别是不存在、不够、无错误
        public enum ERRORCODE { NOTEXISTENCE, NOTENOUGH, SUCCESS};
        //若表不存在，创建表，请尽早调用
        public static void BuildDatabase()
        {
            // Get a reference to the SQLite database
            if (((App)App.Current).conn == null)
            {
                ((App)App.Current).conn = new SQLiteConnection("SQLData.db");
            }
            string sql = @"CREATE TABLE IF NOT EXISTS
                         MaterialItem (
                                    Name            VARCHAR( 100 ) PRIMARY KEY NOT NULL, 
                                    Number          DOUBLE,
                                    Unit            VARCHAR( 100 ),
                                    PurchaseDate    DATE,
                                    WarrantPeriod   DOUBLE,
                                    Price           DOUBLE,
                                    Comment         VARCHAR( 100 )  
                    );";
            using (var statement = ((App)App.Current).conn.Prepare(sql))
            {
                statement.Step();
            }
        }

        //插入给定的项，成功返回true，失败意味着已经有重复名称的项存在
        public static bool Insert(materialItem aim)
        {
            if (GetAItem(aim.name) != null)
                return false;
            // SqlConnection was opened in App.xaml.cs and exposed through property conn
            try
            {
                using (var custstmt = ((App)App.Current).conn.Prepare("INSERT INTO MaterialItem (Name, Number, Unit, PurchaseDate, WarrantPeriod, Price, Comment) VALUES (?, ?, ?, ?, ?, ?, ?)"))
                {
                    custstmt.Bind(1, aim.name);
                    custstmt.Bind(2, aim.number);
                    custstmt.Bind(3, aim.unit);
                    custstmt.Bind(4, aim.purchaseDate.ToString());
                    custstmt.Bind(5, aim.warrantPeriod);
                    custstmt.Bind(6, aim.price);
                    custstmt.Bind(7, aim.comment);
                    custstmt.Step();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        //移除给定项，成功返回true，失败意味着没有这个名称的项
        public static bool Remove(string name)
        {
            if (GetAItem(name) == null)
                return false;
            try
            {
                using (var custstmt = ((App)App.Current).conn.Prepare("DELETE FROM MaterialItem WHERE Name = ?"))
                {
                    custstmt.Bind(1, name);
                    custstmt.Step();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        //更新给定项，成功返回true，失败意味着没有这个名称的项
        public static bool Update(string oldItemName, materialItem newItem)
        {
            if (GetAItem(oldItemName) == null)
                return false;
            try
            {                                                           //"UPDATE TodoItem SET Ti = ?, Desc = ?, Time = ?, Pic = ?, Finished = ? WHERE Tid = ?"
                using (var custstmt = ((App)App.Current).conn.Prepare("update MaterialItem set Name = ?, Number = ?, Unit = ?, PurchaseDate = ?, WarrantPeriod = ?, Price = ?, Comment = ? where Name = ?"))
                {
                    custstmt.Bind(1, newItem.name);
                    custstmt.Bind(2, newItem.number);
                    custstmt.Bind(3, newItem.unit);
                    custstmt.Bind(4, newItem.purchaseDate.ToString());
                    custstmt.Bind(5, newItem.warrantPeriod);
                    custstmt.Bind(6, newItem.price);
                    custstmt.Bind(7, newItem.comment);
                    custstmt.Bind(8, oldItemName);
                    custstmt.Step();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        // 不用管这个函数
        // 0: no problem
        // >0: need number
        public static double CheckNumber(string name, double num)
        {
            materialItem aim = GetAItem(name);
            if (aim == null)
                return num;
            return num - aim.number;
        }

        // 改变一个材料的数量，注意是相对值，公式如下
        // new number = old number + num
        public static ERRORCODE ChangeNumber(string name, double num)
        {
            materialItem aim = GetAItem(name);
            if (aim == null)
                return ERRORCODE.NOTEXISTENCE;
            if(aim.number + num < 0)
            {
                return ERRORCODE.NOTENOUGH;
            }
            materialItem tmp = new materialItem(aim.name, aim.number + num, aim.unit, aim.purchaseDate, aim.warrantPeriod, aim.price, aim.comment);
            Update(name, tmp);
            return ERRORCODE.SUCCESS;
        }

        // 得到所有的材料，返回一个list
        // 操作码：分别是选取所有项、选取过期项、选取不过期项
        public enum SELECTCODE { ALL, DETERIORATED, FRESH};
        // op = ALL: all items, op = DETERIORATED: all deteriorated items, op = FRESH: all fresh items
        public static List<materialItem> GetItems(SELECTCODE op = SELECTCODE.ALL)
        {
            List<materialItem> result = new List<materialItem>();
            using (var custstmt = ((App)App.Current).conn.Prepare("SELECT Name, Number, Unit, PurchaseDate, WarrantPeriod, Price, Comment FROM MaterialItem"))
            {
                while (custstmt.Step() == SQLiteResult.ROW)
                {
                    string temp = (string)custstmt[3];
                    string[] temparr = temp.Split('/', ' ',':');
                    materialItem beCheck = new materialItem(
                            (string)custstmt[0],
                            (double)custstmt[1],
                            (string)custstmt[2],
                            new DateTime(int.Parse(temparr[0]), int.Parse(temparr[1]), int.Parse(temparr[2]), int.Parse(temparr[3]), int.Parse(temparr[4]), int.Parse(temparr[5])),
                            (double)custstmt[4],
                            (double)custstmt[5],
                            (string)custstmt[6]
                        );
                    if(op == SELECTCODE.ALL)
                    {
                        result.Add(beCheck);
                    }
                    else if (op == SELECTCODE.DETERIORATED && beCheck.isDeteriorated())
                    {
                        result.Add(beCheck);
                    }
                    else if (op == SELECTCODE.FRESH && !beCheck.isDeteriorated())
                    {
                        result.Add(beCheck);
                    }
                }
            }
            return result;
        }

        //得到指定名称的材料
        public static materialItem GetAItem(string name)
        {
            materialItem result = null;
            using (var custstmt = ((App)App.Current).conn.Prepare("SELECT Name, Number, Unit, PurchaseDate, WarrantPeriod, Price, Comment FROM MaterialItem WHERE Name = ?"))
            {
                custstmt.Bind(1, name);
                while (custstmt.Step() == SQLiteResult.ROW)
                {
                    string temp = (string)custstmt[3];
                    string[] temparr = temp.Split('/', ' ', ':');
                    result = new materialItem(
                            (string)custstmt[0],
                            (double)custstmt[1],
                            (string)custstmt[2],
                            new DateTime(int.Parse(temparr[0]), int.Parse(temparr[1]), int.Parse(temparr[2]), int.Parse(temparr[3]), int.Parse(temparr[4]), int.Parse(temparr[5])),
                            (double)custstmt[4],
                            (double)custstmt[5],
                            (string)custstmt[6]
                        );
                }
            }
            return result;
        }

        //模糊搜索材料名，返回符合条件的list
        public static List<materialItem> SerachName(string str)
        {
            List<materialItem> result = new List<materialItem>();
            using (var custstmt = ((App)App.Current).conn.Prepare("SELECT Name, Number, Unit, PurchaseDate, WarrantPeriod, Price, Comment FROM MaterialItem WHERE Name LIKE ?"))
            {
                custstmt.Bind(1, "%" + str + "%");
                while (custstmt.Step() == SQLiteResult.ROW)
                {
                    string temp = (string)custstmt[3];
                    string[] temparr = temp.Split('/', ' ', ':');
                    materialItem beCheck = new materialItem(
                            (string)custstmt[0],
                            (double)custstmt[1],
                            (string)custstmt[2],
                            new DateTime(int.Parse(temparr[0]), int.Parse(temparr[1]), int.Parse(temparr[2]), int.Parse(temparr[3]), int.Parse(temparr[4]), int.Parse(temparr[5])),
                            (double)custstmt[4],
                            (double)custstmt[5],
                            (string)custstmt[6]
                    );
                    result.Add(beCheck);
                }
            }
            return result;
        }

        //把这个表删干净
        public static void cleanDatabase()
        {
            try
            {
                using (var custstmt = ((App)App.Current).conn.Prepare("DELETE FROM MaterialItem"))
                {
                    custstmt.Step();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
