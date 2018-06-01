using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace midterm_sql_byzfl.Utils
{
    /// <summary>
    /// 倒计时类(发送验证码按钮，点击后，会倒计时60s，之后才能再次点击。不同界面的多个验证码按钮共享这个倒计时时间。)同一手机号码1分钟只能发1条；
    /// </summary>
    public static class CountDown
    {
        /// <summary>
        /// 倒计时60秒
        /// </summary>
        public static int stTimeCount = 60;

        /// <summary>
        /// 倒计时60s方法
        /// </summary>
        /// <param name="btnCode"></param>
        /// <param name="timeCount"></param>
        public static void ShowCountDown(Button btnCode, int timeCount)
        {
            stTimeCount = timeCount;
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
            int count = stTimeCount;
            int i = 0;
            dispatcherTimer.Tick += delegate
            {
                if (count > 0)
                    count--;
                //倒计时:设置按钮的值,以及按钮不可点击
                btnCode.Content = count + " S ";
                btnCode.IsEnabled = false;
                stTimeCount = count;
                if (count == i)
                {
                    //倒计时完成: 设置按钮的值,以及按钮可用
                    dispatcherTimer.Stop();
                    btnCode.Content = "获取验证码";
                    btnCode.IsEnabled = true;
                    stTimeCount = count;
                }
            };
        }
    }
}
