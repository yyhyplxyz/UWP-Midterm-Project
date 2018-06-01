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
using System.Threading.Tasks;
using midterm_sql_byzfl.Utils;
using midterm_project.Services;
using midterm_sql_byzfl.Utils;
using Windows.UI.Notifications;
using midterm_sql_byzfl.Services;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace midterm_sql_byzfl
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class ServicePage : Page
    {

        private ObservableCollection<menuItem> menuItems;
        private userItem thisuser;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            thisuser = (userItem)e.Parameter;
            UserName.Text = "Hello " + thisuser.UserName + "!";
            menuManager.Getmenus("Eastern", menuItems);
            TitleTextBlock.Text = "Eastern Food";

        }

        public ServicePage()
        {
            this.InitializeComponent();
            menuItems = new ObservableCollection<menuItem>();
            
            //UserName.Text = "hello";
        }
        /**
  * 计算两个整数和
  * @param  {int} lval 左操作数
  * @param  {int} rval 右操作数
  * @return {int} 两个整数和
*/
        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
        }

        private async void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Eastern_Food.IsSelected)
            {
                menuManager.Getmenus("Eastern", menuItems);
                TitleTextBlock.Text = "Eastern Food";
            }
            else if (Western_food.IsSelected)
            {
                menuManager.Getmenus("Western", menuItems);
                TitleTextBlock.Text = "Western food";
            }
            else if(Drink.IsSelected)
            {
                menuManager.Getmenus("Drink", menuItems);
                TitleTextBlock.Text = "Drink";
            }
           
            else if (settingItem.IsSelected)
            {
                SettingDialog st = new SettingDialog(this);
                await st.ShowAsync();
            }
        }
       
      
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Eastern_Food.IsSelected = true;
        }

        private async void SignInPassport()
        {
            if (await MicrosoftPassportHelper.MicrosoftPassportAvailableCheckAsync())
            {
                if(await MicrosoftPassportHelper.CreatePassportKeyAsync(thisuser.UserName))
                {
                    var _account = AccountHelper.AddAccount(thisuser.UserName);

                    var dialog = new ContentDialog
                    {
                        Title = "Notice",
                        Content = "You have successfully added this list",
                        IsPrimaryButtonEnabled = true,
                        PrimaryButtonText = "OK",
                    };
                    await dialog.ShowAsync();
                }
            }
            else
            {
                var dialog = new ContentDialog
                {
                    Title = "Notice",
                    Content = "Microsoft Passport is not setup!\n" +
                    "Please go to Windows Settings and set up a PIN to use it.",
                    IsPrimaryButtonEnabled = true,
                    PrimaryButtonText = "OK",
                };
                await dialog.ShowAsync();
            }
        }

        private void ListBoxItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ListBoxItem tapped_item = sender as ListBoxItem;
            if (tapped_item != null && tapped_item.Tag != null && tapped_item.Tag.ToString().Equals("0"))
            {
                MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
            }
        }

        private async void mySearchBox_QuerySubmitted(SearchBox sender, SearchBoxQuerySubmittedEventArgs args)
        {//safeManager.checkInjection(mySearchBox.QueryText)
            if (safeManager.checkInjection(mySearchBox.QueryText))
            {
                var dialog = new ContentDialog
                {
                    Title = "Notice",
                    Content = "Your input is unsafe",
                    IsPrimaryButtonEnabled = true,
                    PrimaryButtonText = "OK",
                };
                await dialog.ShowAsync();
            }
            else
            {
                var tmp = menuManager.SerachName(mySearchBox.QueryText);
                menuItems.Clear();
                foreach (var i in tmp)
                {
                    menuItems.Add(i);
                }
            }
           
        }
    }
}

