using midterm_project.Models;
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
using System.Collections.ObjectModel;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace midterm_sql_byzfl
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class ServicePage : Page
    {

        private ObservableCollection<menuItem> menuItems;

        public ServicePage()
        {
            this.InitializeComponent();
            menuItems = new ObservableCollection<menuItem>();
        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Eastern_Food.IsSelected)
            {
                mymenuManager.Getmenus("eastern", menuItems);
                TitleTextBlock.Text = "Eastern Food";
            }
            else if (Western_food.IsSelected)
            {
                mymenuManager.Getmenus("western", menuItems);
                TitleTextBlock.Text = "Western food";
            }
            else if(Drink.IsSelected)
            {
                mymenuManager.Getmenus("drink", menuItems);
                TitleTextBlock.Text = "Drink";
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Eastern_Food.IsSelected = true;
        }
    }
}

