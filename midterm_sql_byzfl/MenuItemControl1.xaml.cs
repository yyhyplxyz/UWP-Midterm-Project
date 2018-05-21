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
            ContentDialog subscribeDialog = new ContentDialog
            {
                Title = "Have an Order?",
                Content = "This is up to you!",
                CloseButtonText = "Not Now",
                PrimaryButtonText = "OK",
                //SecondaryButtonText = "Try it"

            };
           
            ContentDialogResult result = await subscribeDialog.ShowAsync();
        }
    }
}
