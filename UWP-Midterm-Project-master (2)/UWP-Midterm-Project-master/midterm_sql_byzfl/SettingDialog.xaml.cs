using midterm_project;
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

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“内容对话框”项模板

namespace midterm_sql_byzfl
{
    public sealed partial class SettingDialog : ContentDialog
    {
        Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
        ServicePage _mainPage;
        public SettingDialog(ServicePage mainPage)
        {
            this.InitializeComponent();
            _mainPage = mainPage;
            LoadConfig();
        }

        private void LoadConfig()
        {
            AlwayShowNavigation.IsOn = App.AlwaysShowNavigation;
            ThemeDark.IsOn = App.Theme == ApplicationTheme.Dark ? true : false;
        }
        /// 开关按钮

        private void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            if ((sender as ToggleSwitch).Name.Equals("AlwayShowNavigation"))
            {
                if (AlwayShowNavigation.IsOn != App.AlwaysShowNavigation)
                {
                    localSettings.Values["AlwaysShowNavigation"] = AlwayShowNavigation.IsOn;
                    App.AlwaysShowNavigation = AlwayShowNavigation.IsOn;
                    //_mainPage.ShowNavigationBar(AlwayShowNavigation.IsOn); //立即生效
                }
            }
            else
            {
                localSettings.Values["Theme"] = ThemeDark.IsOn ? ApplicationTheme.Dark.ToString() : ApplicationTheme.Light.ToString();
            }
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {

        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {

        }
    }
}
