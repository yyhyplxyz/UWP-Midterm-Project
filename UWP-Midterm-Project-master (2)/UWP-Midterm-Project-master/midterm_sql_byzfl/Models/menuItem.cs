using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Media.Imaging;

namespace midterm_project.Models
{
    //菜单项model
    public class menuItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private char SPLITCHAR = ',';
        
        public string menuName { get; set; } //菜单名 （主键）
        public string Category { get; set; }
        public string description { get; set; }
        public string Image { get; set; }
        public string price { get; set; }
        public string SQLString { get; set; }
        public BitmapImage trueimage { get; set; }
        //两个链表，分别储存一道菜对应的每一种材料和数量
        public List <string> materialName { get; set; }
        public List <double> materialNumber { get; set; }

        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public menuItem()
        {

        }
        //构造函数，给定菜名，材料链表和数量链表
        public menuItem(string Name, List<string> MaterialName, List<double> MaterialNumber, string tcatelogy, string tdescription, string timage, string tprice)
        {
            menuName = Name;
            Category = tcatelogy;
            description = tdescription;
            Image = timage;
            price = tprice;
            materialName = MaterialName;
            materialNumber = MaterialNumber;
            SQLString = null;
          
        }

        public menuItem(string Name, string StringFromSQL, string tcatelogy, string tdescription, string timage, string tprice)
        {
            menuName = Name;
            Category = tcatelogy;
            description = tdescription;
            Image = timage;
            price = tprice;

            materialName = new List<string>();
            materialNumber = new List<double>();

            string[] temparr = StringFromSQL.Split(SPLITCHAR);
            int len = temparr.Count() - 1;
            for(int i = 0; i < len; i += 2)
            {
                materialName.Add(temparr[i]);
                double tempnum = -1;
                double.TryParse(temparr[i + 1], out tempnum);
                materialNumber.Add(tempnum);
            }
            SQLString = StringFromSQL;
            trueimage = new BitmapImage(new Uri("ms-appx:///" + Image + ".jpeg", UriKind.Absolute));
        }

        public string generateSQLSavingString()
        {
            if (SQLString != null)
                return SQLString;
            int len = materialName.Count();
            string res = "";
            for(int i = 0; i < len; i++)
            {
                res += materialName.ElementAt(i) + SPLITCHAR + materialNumber.ElementAt(i) + SPLITCHAR;
            }
            SQLString = res;
            return res;
        }

        public string generateFormulaString()
        {
            string result = "";
            int len = materialName.Count();
            for(int i = 1; i <= len; i++)
            {
                result += "材料" + i + "名称:";
                result += materialName.ElementAt(i - 1);
                result += " / 材料" + i + "数量:";
                result += materialNumber.ElementAt(i - 1) + "";
                if(i != len)
                    result += " ; ";
            }
            return result;
        }
    }
    public class mymenuManager
    {
        public static void Getmenus(string category, ObservableCollection<menuItem> menuItems)
        {
            var allItems = getmenuItems();

            var filteredmenuItems = allItems.Where(p => p.Category == category).ToList();

            menuItems.Clear();

            filteredmenuItems.ForEach(p => menuItems.Add(p));
        }

        private static List<menuItem> getmenuItems()
        {
            var items = new List<menuItem>();

            items.Add(new menuItem() { menuName = "23333", Category="eastern", description= "doro sit amet", price = "￥23.3", Image = "Assets/Financial1.png" });
            items.Add(new menuItem() { menuName = "23333", Category = "eastern", description = "doro sit amet", price = "￥23.3", Image = "Assets/Financial1.png" });
            items.Add(new menuItem() { menuName = "23333", Category = "eastern",  description = "doro sit amet", price = "￥23.3", Image = "Assets/Financial1.png" });
            items.Add(new menuItem() { menuName = "23333", Category = "eastern",  description = "doro sit amet", price = "￥23.3", Image = "Assets/Financial1.png" });
            items.Add(new menuItem() { menuName = "23333", Category = "western", description = "doro sit amet", price = "￥23.3", Image = "Assets/Financial1.png" });
            items.Add(new menuItem() { menuName = "23333", Category = "western", description = "doro sit amet", price = "￥23.3", Image = "Assets/Financial1.png" });
            items.Add(new menuItem() { menuName = "23333", Category = "western", description = "doro sit amet", price = "￥23.3", Image = "Assets/Financial1.png" });
            items.Add(new menuItem() { menuName = "23333", Category = "western", description = "doro sit amet", price = "￥23.3", Image = "Assets/Financial1.png" });
            return items;
        }

    }
}
