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
using midterm_sql_byzfl.Utils;
using midterm_sql_byzfl.Models;
using midterm_project.Services;
using midterm_project.Models;
using System.Diagnostics;
// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace midterm_sql_byzfl
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    /// 
    
    public sealed partial class Login : Page
    {
        bool flag;
        public Login()
        {
            flag = false;
            load();
            this.InitializeComponent();
            getlist();
            Loaded += UserSelection_Loaded;
        }
        private async void load()
        {
            await AccountHelper.LoadAccountListAsync();
        }
        private async void getlist()
        {
            if (flag)
            {
                await MicrosoftPassportHelper.CreatePassportKeyAsync("yao");
            }
        }

        private void RegisterButtonTextBlock_OnPointerPressed(object sender, PointerRoutedEventArgs e)
        {
            
        }
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            BackButton.IsEnabled = this.Frame.CanGoBack;
             flag = false;
            // Check Microsoft Passport is setup and available on this machine
            if (await MicrosoftPassportHelper.MicrosoftPassportAvailableCheckAsync())
            {
                //UsernameTextBox.Text = "233";
                flag = true;
                
            }
            

        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {

            On_BackRequested();
        }

        // Handles system-level BackRequested events and page-level back button Click events
        private bool On_BackRequested()
        {
            if (this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
                return true;
            }
            return false;
        }

        private void UserSelection_Loaded(object sender, RoutedEventArgs e)
        {
            if (AccountHelper.AccountList.Count == 0)
            {
                //If there are no accounts navigate to the LoginPage
                Frame.Navigate(typeof(Login));
            }
            UserListView.ItemsSource = AccountHelper.AccountList;
            UserListView.SelectionChanged += UserSelectionChanged;
        }

        /// <summary>
        /// Function called when an account is selected in the list of accounts
        /// Navigates to the Login page and passes the chosen account
        /// </summary>
        private void UserSelectionChanged(object sender, RoutedEventArgs e)
        {
            if (((ListView)sender).SelectedValue != null)
            {
                Accountofdevice account = (Accountofdevice)((ListView)sender).SelectedValue;
                if (account != null)
                {
                    Debug.WriteLine("Account " + account.Username + " selected!");
                }
                Frame.Navigate(typeof(Login), account);
            }
        }

        /// <summary>
        /// Function called when the "+" button is clicked to add a new user.
        /// Navigates to the Login page with nothing filled out
        /// </summary>
        private void AddUserButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(normallogin));
        }

    }
}
