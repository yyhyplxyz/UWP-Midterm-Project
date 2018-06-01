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
using System.Collections.ObjectModel;
namespace midterm_project.Services
{
    //菜单数据库管理类
    public class menuManager
    {
        //错误码，分别是不存在、不够、无错误
        public enum ERRORCODE { NOTEXISTENCE, NOTENOUGH, SUCCESS };
        //若表不存在，创建表，请尽早调用
        public static void BuildDatabase()
        {
            // Get a reference to the SQLite database
            if (((App)App.Current).conn == null)
            {
                ((App)App.Current).conn = new SQLiteConnection("SQLData.db");
            }
            string sql = @"CREATE TABLE IF NOT EXISTS
                         MenuItem (
                                    Name            VARCHAR( 100 ) PRIMARY KEY NOT NULL, 
                                    Formula         VARCHAR( 1000 ),
                                    Category        VARCHAR( 100 ),
                                    Description     VARCHAR( 100 ),
                                    Image           VARCHAR( 100 ),
                                    Price           VARCHAR( 100 )
                    );";
            using (var statement = ((App)App.Current).conn.Prepare(sql))
            {
                statement.Step();
            }
        }
        //插入给定的项，成功返回true，失败意味着已经有重复名称的项存在
        public static bool Insert(menuItem aim)
        {
            if (GetAItem(aim.menuName) != null)
                return false;
            // SqlConnection was opened in App.xaml.cs and exposed through property conn
            try
            {
                using (var custstmt = ((App)App.Current).conn.Prepare("INSERT INTO MenuItem (Name, Formula, Category, Description, Image, Price) VALUES (?, ?, ?, ?, ?, ?)"))
                {
                    custstmt.Bind(1, aim.menuName);
                    custstmt.Bind(2, aim.generateSQLSavingString());
                    custstmt.Bind(3, aim.Category);
                    custstmt.Bind(4, aim.description);
                    custstmt.Bind(5, aim.Image);
                    custstmt.Bind(6, aim.price);
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
                using (var custstmt = ((App)App.Current).conn.Prepare("DELETE FROM MenuItem WHERE Name = ?"))
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
        public static bool Update(string oldItemName, menuItem newItem)
        {
            if (GetAItem(oldItemName) == null)
                return false;
            try
            {                                                           //"UPDATE TodoItem SET Ti = ?, Desc = ?, Time = ?, Pic = ?, Finished = ? WHERE Tid = ?"
                                                                        //Name, Formula, Category, Description, Image, Price
                using (var custstmt = ((App)App.Current).conn.Prepare("update MenuItem set Name = ?, Formula = ?, Category = ?, Description = ?, Image = ?, Price = ? where Name = ?"))
                {
                    custstmt.Bind(1, newItem.menuName);
                    custstmt.Bind(2, newItem.generateSQLSavingString());
                    custstmt.Bind(3, newItem.Category);
                    custstmt.Bind(4, newItem.description);
                    custstmt.Bind(5, newItem.Image);
                    custstmt.Bind(6, newItem.price);
                    custstmt.Bind(7, oldItemName);
                    custstmt.Step();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        //得到所有的菜，返回一个list
        public static List<menuItem> GetItems()
        {
            List<menuItem> result = new List<menuItem>();
            using (var custstmt = ((App)App.Current).conn.Prepare("SELECT Name, Formula, Category, Description, Image, Price FROM MenuItem"))
            {
                while (custstmt.Step() == SQLiteResult.ROW)
                {
                    menuItem beCheck = new menuItem(
                            (string)custstmt[0],
                            (string)custstmt[1],
                            (string)custstmt[2],
                            (string)custstmt[3],
                            (string)custstmt[4],
                            (string)custstmt[5]
                        );
                    result.Add(beCheck);
                }
            }
            return result;
        }

        //得到指定名称的菜
        public static menuItem GetAItem(string name)
        {
            menuItem result = null;
            using (var custstmt = ((App)App.Current).conn.Prepare("SELECT Name, Formula, Category, Description, Image, Price FROM MenuItem WHERE Name = ?"))
            {
                custstmt.Bind(1, name);
                while (custstmt.Step() == SQLiteResult.ROW)
                {
                    result = new menuItem(
                            (string)custstmt[0],
                            (string)custstmt[1],
                            (string)custstmt[2],
                            (string)custstmt[3],
                            (string)custstmt[4],
                            (string)custstmt[5]
                        );
                }
            }
            return result;
        }

        //模糊搜索菜名，返回符合条件的list
        public static List<menuItem> SerachName(string str)
        {
            List<menuItem> result = new List<menuItem>();
            using (var custstmt = ((App)App.Current).conn.Prepare("SELECT Name, Formula, Category, Description, Image, Price FROM MenuItem WHERE Name LIKE ?"))
            {
                custstmt.Bind(1, "%" + str + "%");
                while (custstmt.Step() == SQLiteResult.ROW)
                {
                    menuItem beCheck = new menuItem(
                            (string)custstmt[0],
                            (string)custstmt[1],
                            (string)custstmt[2],
                            (string)custstmt[3],
                            (string)custstmt[4],
                            (string)custstmt[5]
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
                using (var custstmt = ((App)App.Current).conn.Prepare("DELETE FROM MenuItem"))
                {
                    custstmt.Step();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //根据菜名，做一道菜，自动扣除材料库对应材料，返回错误结构体，说明失败的所有原因，错误结构体声明在后面
        public static errorMessage ServeMenuItem(string name)
        {
            menuItem aim = GetAItem(name);
            if (aim == null)
                return new errorMessage(false, true);
            int len = aim.materialName.Count();
            List<string> needMaterialName = new List<string>();
            List<double> needMaterialNumber = new List<double>();
            List<string> DeterioratedName = new List<string>();
            for (int i = 0; i < len; i++)
            {
                string mname = aim.materialName.ElementAt(i);
                double mnum = aim.materialNumber.ElementAt(i);
                if (materialManager.GetAItem(mname) == null)
                {
                    needMaterialName.Add(mname);
                    needMaterialNumber.Add(mnum);
                    continue;
                }
                if (materialManager.GetAItem(mname).isDeteriorated())
                {
                    needMaterialName.Add(mname);
                    needMaterialNumber.Add(mnum);
                    DeterioratedName.Add(mname);
                    continue;
                }
                double needNum = materialManager.CheckNumber(mname, mnum);
                if (needNum > 0)
                {
                    needMaterialName.Add(mname);
                    needMaterialNumber.Add(needNum);
                }
            }
            if(needMaterialName.Count() == 0)
            {
                for (int i = 0; i < len; i++)
                {
                    string mname = aim.materialName.ElementAt(i);
                    double mnum = aim.materialNumber.ElementAt(i);
                    materialManager.ChangeNumber(mname, -mnum);
                }
                return new errorMessage(true, false);
            }
            else
            {
                return new errorMessage(false, false, needMaterialName, needMaterialNumber);
            }
        }
       
        //错误结构体，返回做不成一道菜的所有原因
        public struct errorMessage
        {
            public bool serveSucessed; //是否成功
            public bool menuNoExist; //没有这道菜
            public List<string> needMaterialName; //需要的材料名称
            public List<double> needMaterialNumber; //需要的材料数量
            public List<string> DeterioratedName; //由于过期不能用的材料
            public errorMessage(bool s, bool m, List<string> name = null, List<double> num = null, List<string> d = null)
            {
                serveSucessed = s;
                menuNoExist = m;
                needMaterialName = name;
                needMaterialNumber = num;
                DeterioratedName = d;
            }
        }
        public static void Getmenus(string category, ObservableCollection<menuItem> menuItems)
        {
            var allItems = GetItems();

            var filteredmenuItems = allItems.Where(p => p.Category == category).ToList();

            menuItems.Clear();

            filteredmenuItems.ForEach(p => menuItems.Add(p));
        }

    }
}
