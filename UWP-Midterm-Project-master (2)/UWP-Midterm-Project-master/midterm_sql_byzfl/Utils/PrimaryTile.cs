using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace midterm_sql_byzfl.Utils
{
    class PrimaryTile
    {
        public string time { get; set; } = "ok";
        public string message { get; set; } = "nothing";
        public string message2 { get; set; } = " At vero eos et accusamus et iusto odio dignissimos ducimus qui blanditiis praesentium voluptatum deleniti atque corrupti quos dolores et quas molestias excepturi sint occaecati cupiditate non provident.";
        public string branding { get; set; } = "name";
        public string appName { get; set; } = "过期的食物  餐饮管理系统";

        public PrimaryTile(string input1, string input2, DateTimeOffset input3)
        {
            time = input1;
            message = input2;
            message2 = (input3.Year.ToString()) + "/" + input3.Month.ToString() + "/" + input3.Day.ToString();
        }
    }
}
