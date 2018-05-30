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
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using midterm_sql_byzfl.Utils;
using midterm_project.Services;
using System.Text.RegularExpressions;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace midterm_sql_byzfl
{
    public sealed partial class MenuItemControl1 : UserControl
    {
        public midterm_project.Models.menuItem mymenuItem { get { return this.DataContext as midterm_project.Models.menuItem; } }

        public MenuItemControl1()
        {
            this.InitializeComponent();
            this.DataContextChanged += (s, e) => Bindings.Update();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            
            string number = await InputTextDialogAsync("数量");
            if (Regex.IsMatch(number, @"^\d+$"))
            {
                var tmp = menuManager.ServeMenuItem(mymenuItem.menuName);
                bool issuccessed = tmp.serveSucessed;
                if (!issuccessed)
                {
                    var toneedname = tmp.needMaterialName;
                    var toneednumber = tmp.needMaterialNumber;

                    string toshow = "";
                    for (int i = 0; i < toneedname.Count(); i++)
                    {
                        toshow += toneedname[i] + " : " + toneednumber[i] + "\n";
                    }
                    var dialog = new ContentDialog
                    {
                        Title = "Notice",
                        Content = "这道菜还差一些材料\n" +
                           toshow,
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
                    Content = "请输入数字",
                    IsPrimaryButtonEnabled = true,
                    PrimaryButtonText = "OK",
                };
                await dialog.ShowAsync();
            }
            //ContentDialogResult result = await subscribeDialog.ShowAsync();
        }
        private async Task<string> InputTextDialogAsync(string title)
        {
            TextBox inputTextBox = new TextBox();
            inputTextBox.AcceptsReturn = false;
            inputTextBox.Height = 32;
            ContentDialog dialog = new ContentDialog();
            dialog.Content = inputTextBox;
            dialog.Title = title;
            dialog.IsSecondaryButtonEnabled = true;
            dialog.PrimaryButtonText = "Ok";
            dialog.SecondaryButtonText = "Cancel";
            if (await dialog.ShowAsync() == ContentDialogResult.Primary)
                return inputTextBox.Text;
            else
                return "";
        }
    }
}
