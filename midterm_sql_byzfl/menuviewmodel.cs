using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Reflection;
using midterm_project.Models;
using midterm_project.Services;
using System.Collections.ObjectModel;
using Microsoft.Toolkit.Uwp.Helpers;

namespace midterm_sql_byzfl
{
   public  class menuviewmodel
    {
        public ObservableCollection<menuItem> staticData;

        public menuviewmodel()
        {
            //staticData = new ObservableCollection<menuItem>();
            Load();
        }
        public IList<menuItem> Data
        {
            get
            {
                return staticData;
            }
            set
            {
                staticData = (ObservableCollection<menuItem>)value;
                //staticData.Add(new menuItem("22", "22", 1));
            }
        }

        public void Load()
        {
            staticData = new ObservableCollection<menuItem>(menuManager.GetItems());
        }

        public async Task loadAsync()
        {
            //await DispatcherHelper.ExecuteOnUIThreadAsync(() => IsLoading = true);
            await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
            {
                staticData.Clear();
                var tmp = menuManager.GetItems();
                foreach (var i in tmp)
                {
                    staticData.Add(i);
                }
            });
        }
    }
}
