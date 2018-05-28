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
    public class materialViewModel
    {
        public ObservableCollection<materialItem> staticData;

        public materialViewModel()
        {
            //staticData = new ObservableCollection<materialItem>();
            Load();
        }
        public IList<materialItem> Data
        {
            get
            {
                return staticData;
            }
            set
            {
                staticData = (ObservableCollection<materialItem>)value;
                //staticData.Add(new materialItem("22", "22", 1));
            }
        }

        public void Load()
        {
            staticData = new ObservableCollection<materialItem>(materialManager.GetItems());
        }

        public async Task loadAsync()
        {
            //await DispatcherHelper.ExecuteOnUIThreadAsync(() => IsLoading = true);
            await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
            {
                staticData.Clear();
                staticData = new ObservableCollection<materialItem>(materialManager.GetItems());
                //IsLoading = false;
            });
        }
    }

}
