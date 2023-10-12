using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryResult.Common
{
    public class DatabaseManager
    {
        public string sqlConnection = "Data Source=103.90.248.49;Initial Catalog=BSI_POC;User ID = sa; Password=P@ssw0rd!23#";
        public SqlCommand cmd;
        public SqlDataReader dReader;
        public SqlTransaction trans;
    }
}
