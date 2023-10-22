using QueryResult.Common;
using QueryResult.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryResult.Controller
{
    public static class DataTableExtensions
    {
        public static List<dynamic> ToDynamic(this DataTable dt)
        {
            var dynamicDt = new List<dynamic>();
            foreach (DataRow row in dt.Rows)
            {
                dynamic dyn = new ExpandoObject();
                dynamicDt.Add(dyn);
                foreach (DataColumn column in dt.Columns)
                {
                    var dic = (IDictionary<string, object>)dyn;
                    dic[column.ColumnName] = row[column];
                }
            }
            return dynamicDt;
        }
    }
    public class MainAction
    {
        DatabaseManager db = new DatabaseManager();
        SqlConnection conn = new SqlConnection();
        SqlDataReader reader = null;
        DataTable dt = new DataTable();

        public void DisplayResult(string tableName)
        {
            try
            {
                dt = new DataTable();
                db.OpenConnection(ref conn);
                db.cmd.CommandText = $"SELECT DISTINCT [CategoryName] FROM {tableName}";
                db.cmd.CommandType = CommandType.Text;
                reader = db.cmd.ExecuteReader();

                dt.Load(reader);

                db.CloseDataReader(reader);
                db.CloseConnection(ref conn);

                //foreach(DataColumn col in dt.Columns)
                //{
                //    Console.Write($"{col.ColumnName}\t");
                //}

                foreach (DataRow dr in dt.Rows)
                {
                    foreach(var item in dr.ItemArray)
                    {
                        Console.Write($"{item}\t");
                    }
                    Console.WriteLine();
                }
            }
            catch(Exception ex)
            {
                db.CloseConnection(ref conn);
                throw new Exception(ex.Message);
            }
        }

        public List<ItemCategory> GetCategories(string tableName)
        {
            List<ItemCategory> results = new List<ItemCategory>();
            try
            {
                dt = new DataTable();
                db.OpenConnection(ref conn);
                db.cmd.CommandText = $"SELECT DISTINCT [CategoryName], [CategoryID] FROM {tableName}";
                db.cmd.CommandType = CommandType.Text;
                reader = db.cmd.ExecuteReader();

                dt.Load(reader);

                db.CloseDataReader(reader); 
                db.CloseConnection(ref conn);

                results = Utility.ConvertDataTableToList<ItemCategory>(dt);

                return results;
            }
            catch (Exception ex)
            {
                db.CloseConnection(ref conn);
                throw new Exception(ex.Message);
            }
        }

        public string GetDBTableName(string listName)
        {
            string output = "";
            try
            {
                db.OpenConnection(ref conn);
                db.cmd.CommandText = $"SELECT TOP 1 [DBTableName] FROM [Sambu_Master].[dbo].[MappingStaging]" +
                    $"WHERE [ListName] = '{listName}'";
                db.cmd.CommandType = CommandType.Text;

                reader = db.cmd.ExecuteReader();
                while(reader.Read())
                {
                    output = reader.GetString(0);
                }
                db.CloseConnection(ref conn);
                db.CloseDataReader(reader);

                return output;
            }
            catch(Exception ex)
            {
                db.CloseConnection(ref conn);
                throw new Exception(ex.Message);
            }
        }

        public List<string> GetDBColumnNames(string listName)
        {
            try
            {
                db.OpenConnection(ref conn);
                db.cmd.CommandText = $"SELECT [DBColumnName] FROM [Sambu_Master].[dbo].[MappingStaging] " +
                    $"WHERE [ListName] = '{listName}'";
                db.cmd.CommandType = CommandType.Text;
                reader = db.cmd.ExecuteReader();

                dt.Load(reader);
                db.CloseDataReader(reader);
                db.CloseConnection(ref conn);

                return Utility.ConvertDataTableToList<string>(dt); 

            }
            catch(Exception ex)
            {
                db.CloseConnection(ref conn);
                throw new Exception(ex.Message);
            }
        }
        public string GetStagingTableName(string listName)
        {
            string output = "";
            try
            {
                db.OpenConnection(ref conn);
                db.cmd.CommandText = $"SELECT TOP 1 [StagingTableName] FROM [Sambu_Master].[dbo].[MappingStaging]" +
                    $"WHERE [ListName] = '{listName}'";
                db.cmd.CommandType = CommandType.Text;
                reader = db.cmd.ExecuteReader();

                while(reader.Read())
                {
                    output = reader.GetString(0);
                }
                db.CloseConnection(ref conn);
                db.CloseDataReader(reader);

                return output;
            }
            catch (Exception ex)
            {
                db.CloseConnection(ref conn);
                throw new Exception(ex.Message);
            }
        }
        public List<string> GetStagingColumns(string listName)
        {
            
            try
            {
                var output = new List<string>();
                db.OpenConnection(ref conn);
                db.cmd.CommandText = $"SELECT [StagingColumnName] FROM [Sambu_Master].[dbo].[MappingStaging] " +
                    $"WHERE [ListName] = '{listName}'";
                db.cmd.CommandType = CommandType.Text;
                reader = db.cmd.ExecuteReader();

                while (reader.Read())
                {
                    output.Add(reader.GetString(0));
                }

                dt.Load(reader);


                //output = Utility.ConvertDataTableToList<string>(dt);

                db.CloseDataReader(reader);
                db.CloseConnection(ref conn);
                return output;
            }
            catch (Exception ex)
            {
                db.CloseConnection(ref conn);
                throw new Exception(ex.Message);
            }
        }
        public void GetDataFromStaging1(string listName)
        {
            string sqlQuery = string.Empty;
            try
            {
                List<string> keys = new List<string>() { "CategoryName", "ConversionUOM"}; 

                var listData = new List<dynamic>();
                var stagingColumns = GetStagingColumns(listName);
                var stagingTableName = GetStagingTableName(listName);
                var dbTableName = GetDBTableName(listName);
                //Console.WriteLine(string.Join(",", stagingColumns));
                //Console.WriteLine(string.Join (",", stagingTableName));
                db.OpenConnection(ref conn);
                
                sqlQuery += $"SELECT DISTINCT {string.Join(", ", keys)} FROM {stagingTableName}";
                db.cmd.CommandText = sqlQuery;
                db.cmd.CommandType = CommandType.Text;
                reader = db.cmd.ExecuteReader();

                dt.Load(reader);
                listData = dt.ToDynamic();
                Console.WriteLine(sqlQuery);

                string checkQuery = string.Empty;
                Console.WriteLine(checkQuery);

                //for(int i = 0; i < listData.Count; i++)
                //{
                //    //for(int j = 0; j < stagingColumns.Count; j++) 
                //    //{
                //    //    Console.WriteLine(listData[i].$"{stagingColumns[j]}".ToString());
                //    //}
                //    Console.WriteLine(listData[i]("CategoryName") + ", " + listData[i].CategoryID);
                //    //foreach(var s in stagingColumns)
                //    //{
                //    //    Console.WriteLine(listData[i].s.ToString());
                //    //}
                //}
                //List<Dictionary<string, object>> ListOfDictionary = new();

                //foreach (dynamic item in listData)
                //{
                //    var dictionary = (IDictionary<string, object>)item;
                //    ListOfDictionary.Add((Dictionary<string, object>)dictionary);
                //    //foreach (var key in keys)
                //    //{

                //    //}
                //    //Console.WriteLine($"{dictionary["CategoryName"]}, {dictionary["CategoryID"]}");
                //}
                //string message = string.Empty;
                #region Testing space

                #endregion

                #region This is the code I want
                List<string> QueryList = new();
                for (int i = 0; i < listData.Count; i++)
                {
                    var dic = (IDictionary<string, object>)listData[i];
                    List<string> conditions = new List<string>();

                    for (int j = 0; j < keys.Count; j++)
                    {
                        object value = new();
                        // Check if the value is a string, and if so, enclose it in single quotes
                        //object value = (dic[keys[j]] is string ? $"'{dic[keys[j]]}'" : dic[keys[j]]);

                        if ((dic[keys[j]] is string) || (dic[keys[j]] is DateTime))
                        {
                            value = ($"'{dic[keys[j]]}'");
                        }
                        else if ((dic[keys[j]] is decimal) || (dic[keys[j]] is float))
                        {
                            value = dic[keys[j]];
                            string? stringValue = value.ToString();
                            stringValue = stringValue.Replace(",", ".");
                            value = stringValue;
                        }
                        else
                        {
                            value = dic[keys[j]];
                        }

                        conditions.Add($"{keys[j]} = {value}");
                    }

                    // Join the conditions using "AND" and build the final query
                    checkQuery = $"SELECT TOP 1 {string.Join(", ", keys)} FROM {dbTableName} WHERE {string.Join(" AND ", conditions)}";
                    QueryList.Add(checkQuery);
                    //Console.Write($"{checkQuery}\n");
                }


                Console.WriteLine();


                Console.WriteLine();
                foreach (var query in QueryList)
                {
                    Console.WriteLine(query);
                }
                #endregion

            }
            catch (Exception ex)
            {
                db.CloseConnection(ref conn);
                throw new Exception(ex.Message);
            }
        }
        public void InsertDataToSPList(string listName)
        {
            try
            {
                var columns = GetDBColumnNames(listName);

                db.OpenConnection(ref conn);

            }
            catch (Exception ex)
            {
                db.CloseConnection(ref conn);
                throw new Exception(ex.Message);
            }
        }
        public void CheckDataExists1(string dbTableName)
        {
            try
            {
                db.OpenConnection(ref conn);
                db.cmd.CommandText = $"SELECT TOP 1 * FROM {dbTableName} WHERE ";
            }
            catch (Exception ex)
            {
                db.CloseConnection(ref conn);
                throw new Exception(ex.Message);
            }
        }
        public bool CheckDataExists(string table, string column, string value)
        {
            bool output = false;
            try
            {
                dt = new DataTable();
                db.OpenConnection(ref conn);
                db.cmd.CommandText = $"SELECT TOP 1 {column} FROM {table} WHERE {column} = '{value}'";
                db.cmd.CommandType = CommandType.Text;

                object res = db.cmd.ExecuteScalar();

                if(res != null)
                {
                    output = true;
                }
                else
                {
                    output = false;
                }

                //Console.WriteLine(res);

                db.CloseConnection(ref conn);
                return output;
            }
            catch (Exception ex)
            {
                db.CloseConnection(ref conn); 
                throw new Exception(ex.Message);
            }
        }
        public void TestingQuery(string listName)
        {
            try
            {
                List<string> keys = GetStagingColumns(listName);
                string query = $"SELECT DISTINCT {string.Join(", ", keys)} FROM Sambu_Staging_RSUP.Mst.item";
                List<Dictionary<string, object>> results = new();

                db.OpenConnection(ref conn);
                db.cmd.CommandType = CommandType.Text;
                db.cmd.CommandText = query;

                reader = db.cmd.ExecuteReader();

                while(reader.Read())
                {
                    Dictionary<string, object> row = new Dictionary<string, object>();
                    foreach(var key in keys)
                    {
                        row[key] = reader.GetValue(key);
                    }
                    results.Add(row);
                }

                db.CloseConnection(ref conn);
                db.CloseDataReader(reader);

                //foreach (var row in results)
                //{
                //    foreach (var kvp in row)
                //    {
                //        Console.WriteLine($"{kvp.Key}: {kvp.Value}");
                //    }
                //    Console.WriteLine("---------------");
                //}

                for (int i = 0; i < results.Count; i++)
                {
                    Dictionary<string, object> row = results[i];

                    for (int j = 0; j < keys.Count; j++)
                    {
                        string key = keys[j];
                        object value = row[key];
                        Console.WriteLine($"myItem[\"{key}\"] = \"{value}\";");
                    }
                    Console.WriteLine("myItem.Update();");
                    Console.WriteLine("---------------");
                }

            }

            catch (Exception ex)
            {
                db.CloseConnection(ref conn);
                throw new Exception(ex.Message);
            }            
        }
        public bool CheckIfHasSPList(string dbTableName)
        {
            try
            {
                db.OpenConnection(ref conn);
                db.cmd.CommandText = $"SELECT TOP 1 HasSPList FROM Sambu_Master.dbo.MappingStaging WHERE DBTableName = '{dbTableName}'";
                db.cmd.CommandType = CommandType.Text;

                reader = db.cmd.ExecuteReader();
                bool hasList = false;
                while(reader.Read())
                {
                    hasList = reader.GetBoolean(0);
                }
                db.CloseConnection(ref conn);
                db.CloseDataReader(reader);

                Console.WriteLine($"Table {dbTableName} Has SP List ? : {hasList}");
                return hasList;
            }
            catch(Exception ex)
            {
                db.CloseConnection(ref conn);
                throw new Exception(ex.Message);
            }
        }
    }
}
