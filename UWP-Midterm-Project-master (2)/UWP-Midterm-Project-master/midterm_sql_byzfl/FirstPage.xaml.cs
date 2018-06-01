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
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.Graphics.Imaging;
using Windows.UI.Xaml.Media.Imaging;
using midterm_sql_byzfl.Utils;
using System.Diagnostics;
using midterm_project.Models;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace midterm_sql_byzfl
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class FirstPage : Page
    {
        public FirstPage()
        {
            this.InitializeComponent();
            ImageBrush imageBrush = new ImageBrush();
            imageBrush.ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/1.jpg", UriKind.Absolute));
            mygrid.Background = imageBrush;
        }

        private void NormalLoginButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(normallogin), false);
        }

        private async void DeviceLoginButton_Click(object sender, RoutedEventArgs e)
        {
         
            // Check Microsoft Passport is setup and available on this machine
            if (await MicrosoftPassportHelper.MicrosoftPassportAvailableCheckAsync())
            {
                if (await MicrosoftPassportHelper.CreatePassportKeyAsync("yao"))
                {
                    Frame.Navigate(typeof(ServicePage), new userItem("firends"));
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
    }
}
