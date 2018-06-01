using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace midterm_project.Models
{
    //材料项model
    public class materialItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public string name { get; set; } //名字 （主键）
        public double number { get; set; } //数量
        public string unit { get; set; } //单位（桶、个？）
        public DateTimeOffset purchaseDate { get; set; } //购入日期
        public double warrantPeriod { get; set; } //保质期（以天为单位，可以带小数）
        public double price { get; set; }//价格
        public string comment { get; set; } //评价
        public DateTime showdate { get; set; }

        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public materialItem() { }

        //构造函数
        public materialItem(string Name, double Number, string Unit, DateTimeOffset PurchaseDate, double WarrantPeriod, double Price, string Comment)
        {
            name = Name;
            number = Number;
            unit = Unit;
            purchaseDate = PurchaseDate;
            warrantPeriod = WarrantPeriod;
            price = Price;
            comment = Comment;
            showdate = purchaseDate.DateTime;
        }

        //返回这个材料是否过期
        public bool isDeteriorated()
        {
            if (HowLongDeteriorated() < 0)
                return true;
            return false;
        }

        //返回这个材料还有多久过期，支持带小数，以天为单位，负数表示已经过期的天数
        public double HowLongDeteriorated()
        {
            return (purchaseDate.AddDays(warrantPeriod) - DateTimeOffset.Now).TotalDays;
        }
    }
}
