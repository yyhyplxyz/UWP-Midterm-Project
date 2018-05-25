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

namespace midterm_sql_byzfl
{
    public class PeopleViewModel
    {
       public  List<userItem> staticData;

       public  PeopleViewModel()
        {
            staticData = new List<userItem>();
            Load();
        }

        public IList<userItem> Data
        {
            get
            {
                return staticData;
            }
        }

        public void Load()
        {
            staticData = userManager.GetAItem();
        }
    }

}
