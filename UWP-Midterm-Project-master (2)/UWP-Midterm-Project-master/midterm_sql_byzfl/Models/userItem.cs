using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.Helpers;
using midterm_project.Services;

namespace midterm_project.Models
{
    public class userItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Authority { get; set; }
        public string Image { get; set; }

        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public userItem() { }
        public userItem(string inputUserName, string inputPassword, int inputAuthority, string image)
        {
            UserName = inputUserName;
            Password = inputPassword;
            Authority = inputAuthority;
            Image = image;
        }

       
    }
}
