using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BizWebAPI.Common
{
    public class DbContext
    {

        public readonly static string ERP_Connection = "Data Source='" + Path.Combine(Directory.GetCurrentDirectory(), @"app.db") + "' ";

        // public readonly static string ERP_Connection = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=103.121.78.27)(PORT=1521))(CONNECT_DATA=(SID=FEMAS))); User Id=BDMIT_ERP_101; Password=HaL_EAPI_123";
    }
}
