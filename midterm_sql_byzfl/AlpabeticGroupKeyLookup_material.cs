using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Data.Core;

using midterm_project.Models;
using midterm_project.Services;

namespace midterm_sql_byzfl
{
    class AlpabeticGroupKeyLookup_material :IKeyLookup
    {
        public object GetKey(object instance)
        {
            return ((materialItem)instance).name[0];
        }
    }
}
