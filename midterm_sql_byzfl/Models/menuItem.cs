using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace midterm_project.Models
{
    //菜单项model
    public class menuItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private char SPLITCHAR = ',';
        
        public string menuName { get; set; } //菜单名 （主键）
        public string SQLString { get; set; }

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
        public menuItem() { }

        //构造函数，给定菜名，材料链表和数量链表
        public menuItem(string Name, List<string> MaterialName, List<double> MaterialNumber)
        {
            menuName = Name;
            materialName = MaterialName;
            materialNumber = MaterialNumber;
            SQLString = null;
        }

        public menuItem(string Name, string StringFromSQL)
        {
            menuName = Name;
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
    }
}
