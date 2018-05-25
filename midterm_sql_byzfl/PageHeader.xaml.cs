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
    public sealed partial class PageHeader : UserControl
    {
        public PageHeader()
        {
            InitializeComponent();

          
        }

        /// <summary>
        /// Changes the title bar margin thickness.
        /// </summary>
        private void Current_TogglePaneButtonSizeChanged(Object sender, Rect e)
        {
            titleBar.Margin = new Thickness(e.Right, 0, 0, 0);
        }

        public UIElement HeaderContent
        {
            get { return (UIElement)GetValue(HeaderContentProperty); }
            set { SetValue(HeaderContentProperty, value); }
        }

        /// <summary>
        /// Dependency property as the backing store for HeaderContent. This enables animation, sytling, binding, etc.
        /// </summary>
        public static readonly DependencyProperty HeaderContentProperty =
            DependencyProperty.Register("HeaderContent", typeof(UIElement), typeof(PageHeader), new PropertyMetadata(DependencyProperty.UnsetValue));
    }
}

