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
    class AlpabeticGroupKeyLookup_menu :IKeyLookup
    {
        public object GetKey(object instance)
        {
            return ((menuItem)instance).menuName[0];
        }
    }
}
