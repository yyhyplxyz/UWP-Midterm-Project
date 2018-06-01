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
using midterm_project.Services;
using midterm_project.Models;
using System.Threading.Tasks;
using midterm_sql_byzfl.Utils;
using midterm_sql_byzfl.Services;
// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace midterm_sql_byzfl
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class normallogin : Page
    {
        public string UserName;
        public string pass;
        TileManager testTile = new TileManager();
        public normallogin()
        {
            this.InitializeComponent();

            testTile.circulationUpdate();


        }
        bool check;
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
           // check = (bool)e.Parameter;
           

        }

        private void PassportSignInButton_Click(object sender, RoutedEventArgs e)
        {
            UserName = username.Text;
            pass = passw.Password;
            if (!safeManager.checkInjection(UserName))
            {
                ErrorMessage.Text = "The input is unsafe";
            }
              else if (userManager.isExist(UserName))
            {
                userItem thisuser = userManager.GetAItem(UserName);
                if(userManager.check(UserName, pass))
                {
                    if(check)
                    {
                        AccountHelper.AddAccount(thisuser.UserName);
                    }
                    if (thisuser.Authority == 1)
                    Frame.Navigate(typeof(ServicePage), thisuser);
                    else
                        Frame.Navigate(typeof(userlistpage), thisuser);
                }
                else
                {
                    ErrorMessage.Text = "The password is incorrect";
                }
            }
            else
            {
                ErrorMessage.Text = "The user does not exist";
            }
        }

        private void RegisterButtonTextBlock_OnPointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Frame.Navigate(typeof(RegisterPage));
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(FirstPage));
        }
    }
}
