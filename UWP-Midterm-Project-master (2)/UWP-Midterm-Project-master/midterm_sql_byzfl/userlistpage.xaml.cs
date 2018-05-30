using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using midterm_project.Services;
using Windows.UI.Core;
using Microsoft.Toolkit.Uwp.Helpers;
using midterm_project.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Telerik.Core;
using System.Windows;



// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace midterm_sql_byzfl
{
    public class materialEditingViewModel : ViewModelBase
    {
        public materialEditingViewModel()
        {
            this.Data = this.LoadData();
        }

        private ObservableCollection<userItem> LoadData()
        {
            var data = new ObservableCollection<userItem>();

            Action<XElement, userItem> childElementAction = (el, a) =>
            {
                string value = el.Value;

                switch (el.Name.LocalName)
                {
                    case "UserName":
                        a.UserName = value;
                        break;
                    case "Password":
                        a.Password = value;
                        break;
                    case "Authority":
                        a.Authority = Convert.ToInt32(bool.Parse(value));
                        break;

                }
                
            };

            Action<userItem> elementAction = (q) =>
            {
                data.Add(q);
            };

            return data;
        }

        public ObservableCollection<userItem> Data { get; set; }

    }
        public sealed partial class userlistpage : Page
    {
       
        public userlistpage()
        {
            InitializeComponent();
            this.DataContext = new materialEditingViewModel();
        }

        private void ViewDetails_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void AddOrder_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CommandBar_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void CreateCustomer_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Eastern_Food.IsSelected)
            {
                //myzoomview.
                userlistview.Visibility = Visibility.Collapsed;
                materiallistview.Visibility = Visibility.Visible;
                // mymenuManager.Getmenus("eastern", menuItems);
                menulistview.Visibility = Visibility.Collapsed;
                TitleTextBlock.Text = "原材料";
            }
            else if (Western_food.IsSelected)
            {
                //mymenuManager.Getmenus("western", menuItems);
                userlistview.Visibility = Visibility.Collapsed;
                materiallistview.Visibility = Visibility.Collapsed;
                menulistview.Visibility = Visibility.Visible;
                TitleTextBlock.Text = "菜单";
            }
            else if (Drink.IsSelected)
            {
                //mymenuManager.Getmenus("drink", menuItems);
                userlistview.Visibility = Visibility.Visible;
                materiallistview.Visibility = Visibility.Collapsed;
                // mymenuManager.Getmenus("eastern", menuItems);
                menulistview.Visibility = Visibility.Collapsed;
                TitleTextBlock.Text = "用户信息";
            }
            else if (settingItem.IsSelected)
            {
                MySplitView.IsPaneOpen = false;
                myhelp();
                
            }
        }
        private async Task myhelp()
        {
            Setting_Dialog_adminstratior st = new Setting_Dialog_adminstratior(this);
            await st.ShowAsync();
        }
        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
        }

        private void ListBoxItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ListBoxItem tapped_item = sender as ListBoxItem;
            if (tapped_item != null && tapped_item.Tag != null && tapped_item.Tag.ToString().Equals("0"))
            {
                MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
            }
        }
    }
}
