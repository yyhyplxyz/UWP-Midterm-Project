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
using midterm_sql_byzfl.Services;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage.Streams;

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
                int numericalnumber = Int32.Parse(number);
                for(int g = 0; g < numericalnumber; g++)
                {
                    var tmp = menuManager.ServeMenuItem(mymenuItem.menuName);
                    bool issuccessed = tmp.serveSucessed;

                    if (!issuccessed)
                    {
                        var toneedname = tmp.needMaterialName;
                        var toneednumber = tmp.needMaterialNumber;
                        var badmaterial = tmp.DeterioratedName;
                        string toshow = "";
                        for (int i = 0; i < toneedname.Count(); i++)
                        {
                            toshow += toneedname[i] + " : " + toneednumber[i] + "\n";
                          
                        }
                        if(badmaterial.Count() != 0)
                        {
                            toshow += "过期了的材料:\n";
                            for (int i = 0; i < badmaterial.Count(); i++)
                            {
                                toshow += badmaterial[i];
                                toshow += " ";
                            }
                        }
                        var dialog = new ContentDialog
                        {
                            Title = "Notice",
                            Content = "这份订单还差一些材料\n" +
                               toshow,
                            IsPrimaryButtonEnabled = true,
                            PrimaryButtonText = "OK",
                        };
                        await dialog.ShowAsync();
                        return;
                    }
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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DataTransferManager.ShowShareUI();
            DataTransferManager dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += new TypedEventHandler<DataTransferManager, DataRequestedEventArgs>(this.onShareDataRequested);
            //shareManager.shareIt("title", "description", "zhangflu is handsome!\n Wow! handsome boy!", null);
        }
        private void onShareDataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            var dp = args.Request.Data;
            var def = args.Request.GetDeferral();
            RandomAccessStreamReference photo;
            photo = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///" + mymenuItem.Image));
            dp.Properties.Title = mymenuItem.menuName;
            dp.Properties.Description = mymenuItem.description;
            dp.SetBitmap(photo);
            dp.SetText(mymenuItem.Category);
            def.Complete();
        }
    }
}
