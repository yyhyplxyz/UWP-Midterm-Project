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
        }

        public ServicePage()
        {
            this.InitializeComponent();
            menuItems = new ObservableCollection<menuItem>();
            UserName.Text = "Hello ";
        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Eastern_Food.IsSelected)
            {
                menuManager.Getmenus("a", menuItems);
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
            else if (ADD.IsSelected)
            {
                myhelp();
            }
            else if (settingItem.IsSelected)
            {
                MySplitView.IsPaneOpen = false;
                myhelp2();
            }
        }
        private async Task myhelp2()
        {
            SettingDialog st = new SettingDialog(this);
            await st.ShowAsync();
        }
        private async Task myhelp()
        {
            var dialog = new ContentDialog
            {
                Title = "Notice",
                Content = "Are you sure you want to login with this device later?",
                IsPrimaryButtonEnabled = true,
                PrimaryButtonText = "OK",
                SecondaryButtonText = "cancel",
             
            };
            dialog.PrimaryButtonClick += (_s, _e) => { SignInPassport(); };
            await dialog.ShowAsync();
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
    }
}

