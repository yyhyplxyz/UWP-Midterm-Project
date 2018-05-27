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
    public sealed partial class CollapsibleSearchBox : UserControl
    {
        private AutoSuggestBox myAutoSuggestBox;
        public AutoSuggestBox AutoSuggestBox
        {
            get { return myAutoSuggestBox; }
            private set { myAutoSuggestBox = value; }
        }
        public CollapsibleSearchBox()
        {
            this.InitializeComponent();
            //Loaded += CollapsableSearchBox_Loaded;
            //Window.Current.SizeChanged += Current_SizeChanged;
            myAutoSuggestBox = searchBox;
        }

        private void SearchButton_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void SearchBox_LostFocus(object sender, RoutedEventArgs e)
        {
            //SetState(Window.Current.Bounds.Width);
            searchButton.IsChecked = false;
        }
    }
}
