using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace QueryResult.Controller
{
    public class TestingController
    {
        SyncDataController syncDataController = new SyncDataController();
        public void TestAction()
        {
            string TableName = "Sambu_Nintex.Mst.item_sub_category";
            List<string> StagingColumns = syncDataController.GetStagingColumnNames(TableName);
            List<Dictionary<string, object>> QueryResults = syncDataController.GetDataFromStaging(TableName);

            for (int i = 0; i < QueryResults.Count; i++)
            {
                Dictionary<string, object> row = QueryResults[i];
                for (int j = 0; j < StagingColumns.Count; j++)
                {
                    string key = StagingColumns[j];
                    object value = row[key];
                    Console.WriteLine($"myItem[\"{key}\"] = \"{value}\";");
                }
                Console.WriteLine("---------------");
            }
        }
    }
}
