using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Linq;
using Telerik.Data.Core;

using midterm_project.Models;
using midterm_project.Services;

namespace midterm_sql_byzfl
{
    public class AlpabeticGroupKeyLookup : IKeyLookup
    {
        public object GetKey(object instance)
        {
            return ((userItem)instance).UserName[0];
        }
    }

}
