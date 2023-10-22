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
            #region Display the data from staging table
            //string TableName = "Sambu_Nintex.Mst.item_sub_category";
            //List<string> StagingColumns = syncDataController.GetStagingColumnNames(TableName);
            //List<Dictionary<string, object>> QueryResults = syncDataController.GetDataFromStaging(TableName);

            //for (int i = 0; i < QueryResults.Count; i++)
            //{
            //    Dictionary<string, object> row = QueryResults[i];
            //    for (int j = 0; j < StagingColumns.Count; j++)
            //    {
            //        string key = StagingColumns[j];
            //        object value = row[key];
            //        Console.WriteLine($"myItem[\"{key}\"] = \"{value}\";");
            //    }
            //    Console.WriteLine("---------------");
            //}
            #endregion

            #region build query to check the data in the destination table
            //string TableName = "Sambu_Nintex.Mst.item_sub_category";
            //List<string> DBColumns = syncDataController.GetDBColumnNames(TableName);
            //List<string> StagingColumns = syncDataController.GetStagingColumnNames(TableName);
            //List<Dictionary<string, object>> QueryResults = syncDataController.GetDataFromStaging(TableName);

            //List<string> QueryList = new();
            //for(int i = 0; i < QueryResults.Count; i++)
            //{
            //    Dictionary<string, object> row = QueryResults[i];
            //    List<string> conditions = new List<string>();


            //    for (int j = 0; j < StagingColumns.Count; j++)
            //    {
            //        string key = StagingColumns[j];
            //        object value = row[key];

            //        if((value is string) || (value is DateTime))
            //        {
            //            conditions.Add($"{DBColumns[j]} = '{value}'");
            //        }
            //        else
            //        {
            //            conditions.Add($"{DBColumns[j]} = {value}");
            //        }
            //        //Console.WriteLine($"myItem[\"{key}\"] = \"{value}\";");
            //        //Console.WriteLine($"SELECT TOP 1 {string.Join(", ", DBColumns)} FROM {TableName} WHERE {string.Join(", ", conditions)}");
            //    }
            //    string query = $"SELECT TOP 1 {string.Join(", ", DBColumns)} FROM {TableName} WHERE {string.Join(" AND ", conditions)}";
            //    QueryList.Add(query);
            //    //Console.WriteLine($"SELECT TOP 1 {string.Join(", ", DBColumns)} FROM {TableName} WHERE {string.Join(" AND ", conditions)}");
            //    //Console.WriteLine("---------------");
            //}

            //Console.WriteLine(string.Join("\n", QueryList));
            #endregion

            #region Display the check data query
            //string TableName = "Sambu_Nintex.Mst.item_sub_category";
            //List<string> QueryList = syncDataController.BuildQuery(TableName);
            //Console.WriteLine(string.Join("\n", QueryList));
            #endregion

            #region Check if the data exists
            string TableName = "Sambu_Nintex.Mst.item_sub_category";
            bool con = syncDataController.CheckDataExists(TableName);
            string message = (con == true) ? "Data sudah ada di database" : "Data belum ada di database";
            Console.WriteLine(message);
            #endregion
        }
    }
}
