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
using Windows.Storage.Pickers;
using Windows.UI.Xaml.Media.Imaging;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.Storage;
using System.Text.RegularExpressions;
using Windows.UI.Popups;
using midterm_project.Services;
using midterm_project.Models;
using System.Threading;
using System.Net.Http;
using Yunpian.Sdk;
using Yunpian.Sdk.Model;
using midterm_sql_byzfl.Utils;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace midterm_sql_byzfl
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class RegisterPage : Page
    {
        public RegisterPage()
        {
            this.InitializeComponent();
            apiKey = "766ed53f0d398e4f81de02338193df3d";
        }
        String sendcode;
        string apiKey;
        string filename;
        private async Task<BitmapImage> LoadImage(StorageFile file)
        {
            BitmapImage bitmapImage = new BitmapImage();
            filename = file.Name;
            FileRandomAccessStream stream = (FileRandomAccessStream)await file.OpenAsync(FileAccessMode.Read);
            bitmapImage.SetSource(stream);
            return bitmapImage;
        }

        private  async void selectButton_Click(object sender, RoutedEventArgs e)
        {

            FileOpenPicker fo = new FileOpenPicker();
            fo.FileTypeFilter.Add(".png");
            fo.FileTypeFilter.Add(".jpg");
            fo.SuggestedStartLocation = PickerLocationId.Desktop;

            var f = await fo.PickSingleFileAsync();
            BitmapImage img = new BitmapImage();
            img = await LoadImage(f);
            mypic.Source = img;
        }

        private async void Submit_ClickAsync()
        {
            
            //UserName Validation  
            if (!Regex.IsMatch(TxtUserName.Text.Trim(), @"^[A-Za-z_][a-zA-Z0-9_\s]*$"))
            {
                var dialog = new MessageDialog("Invalid UserName");
                await dialog.ShowAsync();
               
            }
            else if(textsmscode.Text != sendcode)
            {
                var dialog = new MessageDialog("Invalid SMScode");
                await dialog.ShowAsync();
            }

            //Password length Validation  
            else if (TxtPwd.Password.Length < 6)
            {
                var dialog = new MessageDialog("Password length should be minimum of 6 characters!");
                await dialog.ShowAsync();
            } 
           

            //Phone Number Length Validation  
            else if (TxtPhNo.Text.Length != 11)
            {
                var dialog = new MessageDialog("Invalid PhonNo");
                await dialog.ShowAsync();
            }

            //EmailID validation  
            else if (!Regex.IsMatch(TxtEmail.Text.Trim(), @"^([a-zA-Z_])([a-zA-Z0-9_\-\.]*)@(\[((25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9][0-9]|[0-9])\.){3}|((([a-zA-Z0-9\-]+)\.)+))([a-zA-Z]{2,}|(25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9][0-9]|[0-9])\])$"))
            {
                var dialog = new MessageDialog("Invalid EmailId");
                await dialog.ShowAsync();
            }
            //After validation success ,store user detials in isolated storage  
            else if (TxtUserName.Text != "" && TxtPwd.Password != "" &&  TxtEmail.Text != "" &&   TxtPhNo.Text != "")
            {
                if(userManager.GetAItem(TxtUserName.Text) == null)
                {
                    var dialog = new MessageDialog("Congrats! your have successfully Registered.");
                    await dialog.ShowAsync();
                    userManager.Insert(new userItem(TxtUserName.Text, TxtPwd.Password, 1, filename,TxtPhNo.Text, TxtEmail.Text));
                    Frame.Navigate(typeof(ServicePage));
                }
                else
                {
                    var dialog = new MessageDialog("Sorry! user name is already existed.");
                    await dialog.ShowAsync();
                }
            }
            else
            {
                var dialog = new MessageDialog("Please enter all details");
                await dialog.ShowAsync();
            }
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            Submit_ClickAsync();
        }

       
        public YunpianClient sendsms()
        {
            //初始化clnt,使用单例方式
            var clnt = new YunpianClient(apiKey).Init();
            var rannumber = registerhelper.GetRandomString(6);
            sendcode = rannumber;
            //发送短信API
            var param = new Dictionary<string, string>
            {
                [Const.Mobile] = TxtPhNo.Text,
                [Const.Text] = "【朱逸渠】您的验证码是" + rannumber
            };
            var r = clnt.Sms().SingleSend(param);
            clnt.Dispose();
            return clnt;
        }
    
        private void btnCode_Click(object sender, RoutedEventArgs e)
        {
            sendsms();
        }

        private void sendcode_Click(object sender, RoutedEventArgs e)
        {
            var tmp = sendsms();
            if (CountDown.stTimeCount > 0)
            {
                CountDown.ShowCountDown(btnCode, CountDown.stTimeCount);
            }
            
            note.Text = "验证码已发送至" + TxtPhNo.Text;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(normallogin));
        }
    }
}
