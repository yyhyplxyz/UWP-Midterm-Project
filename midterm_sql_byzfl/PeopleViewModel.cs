using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Reflection;
using midterm_project.Models;
using midterm_project.Services;
using System.Collections.ObjectModel;
using Microsoft.Toolkit.Uwp.Helpers;

namespace midterm_sql_byzfl
{
    public class PeopleViewModel
    {
       public  ObservableCollection<userItem> staticData;
       
       public  PeopleViewModel()
        {
            //staticData = new ObservableCollection<userItem>();
            Load();
        }
        public IList<userItem> Data
        {
            get
            {
                return staticData;
            }
            set
            {
                staticData = (ObservableCollection<userItem>)value;
                //staticData.Add(new userItem("22", "22", 1));
            }
        }

        public void Load()
        {
            staticData = new ObservableCollection<userItem>(userManager.GetAItem());
        }

        public async Task loadAsync()
        {
            //await DispatcherHelper.ExecuteOnUIThreadAsync(() => IsLoading = true);
            await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
            {
                staticData.Clear();
                staticData = new ObservableCollection<userItem>(userManager.GetAItem());
                //IsLoading = false;
            });
        }
    }

}
