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
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Windows.UI.Popups;
using Windows.UI.Xaml.Media.Imaging;
using System.Text.RegularExpressions;
using System.Xml;
using System.Diagnostics;


// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“内容对话框”项模板

namespace midterm_sql_byzfl
{
    public sealed partial class Setting_Dialog_adminstratior : ContentDialog
    {
        Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
        userlistpage _mainPage;
       // string Text;
        public Setting_Dialog_adminstratior(userlistpage mainPage)
        {
            this.InitializeComponent();
            _mainPage = mainPage;
            LoadConfig();
            //Text = "广州";
        }
        private void LoadConfig()
        {
           // AlwayShowNavigation.IsOn = App.AlwaysShowNavigation;
            ThemeDark.IsOn = App.Theme == ApplicationTheme.Dark ? true : false;
        }
        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
           
        }
        async void queryWeatherAsync(string city)
        {
            string url = "https://api.seniverse.com/v3/weather/now.json?key=earec8f1zhhxsyki&location=" + city + "&language=zh-Hans&unit=c";
            Uri uri = new Uri(url);
            HttpClient client = new HttpClient();
            try
            {
                string result = await client.GetStringAsync(uri);
                if (result == "")
                    return;
                JObject jo = JObject.Parse(result);
                string[] values = jo.Properties().Select(item => item.Value.ToString()).ToArray();
                JArray message = (JArray)JsonConvert.DeserializeObject(values[0]);
                string text = message[0]["now"]["text"].ToString();
                string code = message[0]["now"]["code"].ToString();
                string temper = message[0]["now"]["temperature"].ToString();
                weatherPicture.Source = new BitmapImage(new Uri("ms-appx:///Assets/weather/" + code + ".png"));
                cityName.Text = message[0]["location"]["name"].ToString();
                date.Text = DateTime.Now.Year.ToString() + "年" + DateTime.Now.Month.ToString() + "月" + DateTime.Now.Day.ToString() + "日";
                details.Text = text;
                temperature.Text = temper + "°C";
            }
            catch (Exception)
            {
                string errorMsg = "输入非法！\n";

                await new MessageDialog(errorMsg).ShowAsync();
            }
        }
        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
        private void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
          
                localSettings.Values["Theme"] = ThemeDark.IsOn ? ApplicationTheme.Dark.ToString() : ApplicationTheme.Light.ToString();
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
          
        }

        private void TextBox1_TextChanged(object sender, TextChangedEventArgs e)
        {
            weatherPicture.Source = null;
            cityName.Text = "";
            date.Text = "";
            details.Text = "";
            temperature.Text = "";
            queryWeatherAsync(queryWeather.Text.Trim());
        }
    }
}
