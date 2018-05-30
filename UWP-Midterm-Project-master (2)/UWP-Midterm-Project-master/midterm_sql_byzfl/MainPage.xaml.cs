using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using midterm_project.Models;
using midterm_project.Services;
using midterm_sql_byzfl.Services;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace midterm_project
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();    
        }
        //测试代码,可以删掉
        //测试材料数据库
        private void testMaterial_Click(object sender, RoutedEventArgs e)
        {
            string result = "";
            materialManager.BuildDatabase();
            materialManager.Insert(new materialItem("egg", 100, "one", DateTimeOffset.Now, 2, 12.5, "good"));
            materialManager.Insert(new materialItem("b", 2, "one", DateTimeOffset.Now.AddDays(2), 1, 12.5, "goodddddddd"));
            materialManager.Insert(new materialItem("c", 3, "one", DateTimeOffset.Now, 2, 12.5, "good"));
            materialManager.Insert(new materialItem("d", 4, "one", DateTimeOffset.Now, 2, 12.5, "good"));
            materialManager.Insert(new materialItem("e", 5, "one", DateTimeOffset.Now, 2, 12.5, "good"));
            materialManager.Insert(new materialItem("f", 6, "one", DateTimeOffset.Now, 2, 12.5, "good"));
            materialManager.Insert(new materialItem("apple", 3, "one", DateTimeOffset.Now, 2, 12.5, "good"));
            materialManager.Insert(new materialItem("banana", 4, "one", DateTimeOffset.Now, 2, 12.5, "good"));
            materialManager.Insert(new materialItem("water", 5, "one", DateTimeOffset.Now, 2, 12.5, "good"));
            materialManager.Insert(new materialItem("det", 100, "one", new DateTimeOffset(1999, 10, 10, 0, 0, 0, new TimeSpan()), 0, 12.5, "good"));
            result += materialManager.ChangeNumber("fuck", 100);
            result += materialManager.ChangeNumber("egg", -1000000);
            result += materialManager.ChangeNumber("egg", 10);
            materialManager.Remove("c");
            List<materialItem> res = materialManager.GetItems(materialManager.SELECTCODE.FRESH);

            for (int i = 0; i < res.Count; i++)
            {
                result += res.ElementAt(i).name;
                result += "  ";
                result += res.ElementAt(i).number;
                result += "  ";
                result += res.ElementAt(i).purchaseDate;
                result += "\n";
            }
            result += "=====================\n";
            //result += materialManager.GetAItem("egg").name + "\n";
            result += materialManager.GetAItem("fuck") == null;
            result += "\n";
            result += "=====================\n";
            result += materialManager.GetAItem("b").HowLongDeteriorated();
            result += "\n=====================\n";
            res = materialManager.SerachName("e");
            for (int i = 0; i < res.Count; i++)
            {
                result += res.ElementAt(i).name;
                result += "  ";
                result += res.ElementAt(i).price;
                result += "  ";
                result += res.ElementAt(i).purchaseDate;
                result += "\n";
            }
            test.Text = result;
        }
        //测试菜单数据库
        private void testMenu_Click(object sender, RoutedEventArgs e)
        {
            string result = "";
            menuItem t1 = new menuItem("t1", "apple,1.5,banana,2.5,water,3.5,", "a" , "hello", "zzz", "100$");
            menuItem t2 = new menuItem("t2", "fuck,1.5,banana,2.5,water,3.5,", "a", "hello", "zzz", "100$");
            menuItem t3 = new menuItem("t3", "apple,1.5,banana,2.5,water,100000,", "a", "hello", "zzz", "100$");
            menuItem t4 = new menuItem("t4", "apple,1.5,banana,2.5,water,100000,", "a", "hello", "zzz", "100$");
            menuItem t5 = new menuItem("t5", "det,1.5,", "a", "hello", "zzz", "100$");
            result += t1.generateSQLSavingString();
            menuManager.BuildDatabase();
            menuManager.Insert(t1);
            menuManager.Insert(t2);
            menuManager.Insert(t3);
            menuManager.Insert(t4);
            menuManager.Insert(t5);
            menuManager.Remove("t4");
            menuManager.Update("t3", new menuItem("t3", "apple,1.5,banana,2.5,water,1999999,", "a", "hello", "zzz", "100$"));
            result += "\n" + menuManager.GetAItem("t3").generateFormulaString() + "\n";
            menuManager.ServeMenuItem("t1");
            menuManager.ServeMenuItem("t1");
            var temp = menuManager.ServeMenuItem("t1");
            List<menuItem> res = menuManager.GetItems();
            for (int i = 0; i < res.Count; i++)
            {
                result += res.ElementAt(i).menuName;
                result += "  ";
                result += res.ElementAt(i).SQLString;
                result += "  ";
                result += res.ElementAt(i).description;
                result += "  ";
                result += res.ElementAt(i).Category;
                result += "  ";
                result += res.ElementAt(i).price;
                result += "\n";
            }
            result += "=====================\n";
            result += temp.serveSucessed + "\n";
            temp = menuManager.ServeMenuItem("t2");
            for(int i = 0; i < temp.needMaterialName.Count; i++)
            {
                result += temp.needMaterialName.ElementAt(i) + "  " + temp.needMaterialNumber.ElementAt(i) + "\n";
            }
            result += "=====================\n";
            temp = menuManager.ServeMenuItem("t5");
            for (int i = 0; i < temp.needMaterialName.Count; i++)
            {
                result += temp.needMaterialName.ElementAt(i) + "  " + temp.needMaterialNumber.ElementAt(i) + "\n";
            }
            test2.Text = result;
        }

        //清除三个表的数据
        private void clearDatabase_Click(object sender, RoutedEventArgs e)
        {
            materialManager.cleanDatabase();
            menuManager.cleanDatabase();
            userManager.cleanDatabase();
        }

        private void testUser_Click(object sender, RoutedEventArgs e)
        {
            string result = "";
            userManager.BuildDatabase();
            userManager.Insert(new userItem("yao", "yao", 1, "hello"));
            userManager.Insert(new userItem("zhang", "zhang", 0, "hello"));
            userManager.Insert(new userItem("new", "new", 1, "hello"));
            userManager.Insert(new userItem("hello", "hello", 0, "hello"));
            List<userItem> res = userManager.GetAItem();
            for(int i = 0; i < res.Count; i++)
            {
                result += res.ElementAt(i).UserName;
                result += "  ";
                result += res.ElementAt(i).Password;
                result += "  ";
                result += res.ElementAt(i).Authority;
                result += "  ";
                result += res.ElementAt(i).Image;
                result += "\n";
            }
            userManager.Update("zhang",new userItem("zhangflu", "handsome", 1, "cool"));
            userItem temp = userManager.GetAItem("zhangflu");
            result += temp.UserName;
            result += "  ";
            result += temp.Password;
            result += "  ";
            result += temp.Authority;
            result += "\n";
            userManager.Remove("hello");
            bool bool1 = userManager.isAdministrator("root");
            bool bool2 = userManager.isAdministrator("yao");
            result += bool1;
            result += "\n";
            result += bool2;
            result += "\n";
            result += userManager.check("zhangflu", "handsome");
            result += userManager.check("root", "hhh");
            result += userManager.check("r", "hhh");
            result += "\n";
            result += safeManager.checkInjection("update MaterialItem set Name = \'hhh\' wherE Name = \'egg\'");
            result += safeManager.checkInjection("i am handsome");
            test3.Text = result;
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(midterm_sql_byzfl.ServicePage));
        }

        private void HyperlinkButton_Click_1(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(midterm_sql_byzfl.Login));
        }

        private void HyperlinkButton_Click_2(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(midterm_sql_byzfl.userlistpage));
        }
    }
}
