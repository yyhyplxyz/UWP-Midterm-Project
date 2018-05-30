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

        public normallogin()
        {
            this.InitializeComponent();
        }

        private void PassportSignInButton_Click(object sender, RoutedEventArgs e)
        {
            UserName = username.Text;
            pass = passw.Password;
            if(userManager.isExist(UserName))
            {
                userItem thisuser = userManager.GetAItem(UserName);
                if(thisuser.Password == pass)
                {
                    Frame.Navigate(typeof(ServicePage), thisuser);
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
            myhelp();
        }
        private async Task myhelp()
        {
            var dialog = new ContentDialog
            {
                Title = "提示",
                Content = "请联系老板，杨元昊",
                IsPrimaryButtonEnabled = true,
                PrimaryButtonText="OK"
            };
            await dialog.ShowAsync();
        }
    }
}
