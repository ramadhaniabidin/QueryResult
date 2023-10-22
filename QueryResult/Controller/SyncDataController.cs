using QueryResult.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace QueryResult.Controller
{
    public class SyncDataController
    {
        DatabaseManager db = new DatabaseManager();
        SqlConnection conn = new SqlConnection();
        SqlDataReader? reader;
        DataTable dt = new DataTable();

        public string mappingTable = "Sambu_Master.dbo.MappingStaging";

        public bool CheckSPList(string DBTableName)
        {
            string ListName = string.Empty;
            bool output;

            db.OpenConnection(ref conn);
            db.cmd.CommandText = $"SELECT TOP 1 [ListName] FROM Sambu_Master.dbo.MappingStaging WHERE DBTableName = '{DBTableName}'";
            db.cmd.CommandType = CommandType.Text;

            reader = db.cmd.ExecuteReader();
            while(reader.Read())
            {
                ListName = reader.GetString(0);
            }

            if(ListName != "") 
            {
                output = true;
            }

            else
            {
                output = false;
            }

            //Console.WriteLine($"List Name of the table {DBTableName} is '{ListName}'");
            db.CloseConnection(ref conn);
            db.CloseDataReader(reader);
            return output;
        }
        #region If the destination table has SharePoint List

        #region Get the name of the SharePoint List
        public string GetSPListName(string DBTableName)
        {
            string ListName = string.Empty;
            try
            {
                db.OpenConnection(ref conn);
                db.cmd.CommandText = $"SELECT TOP 1 ListName FROM Sambu_Master.dbo.MappingStaging WHERE DBTableName = '{DBTableName}'";
                db.cmd.CommandType = CommandType.Text;

                reader = db.cmd.ExecuteReader();
                while(reader.Read())
                {
                    ListName = reader.GetString(0);
                }
                db.CloseConnection(ref conn);
                db.CloseDataReader(reader);

                return ListName;
            }
            catch (Exception ex)
            {
                db.CloseConnection(ref conn);
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Get the column names of the destination table
        public List<string> GetDBColumnNames(string DBTableName)
        {
            try
            {
                List<string> output = new List<string>();
                db.OpenConnection(ref conn);
                db.cmd.CommandText = $"SELECT DBColumnName FROM {mappingTable} WHERE DBTableName = '{DBTableName}'";
                db.cmd.CommandType = CommandType.Text;

                reader = db.cmd.ExecuteReader();
                while(reader.Read())
                {
                    output.Add(reader.GetString(0));
                }
                dt.Load(reader);
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
        #endregion

        #region Get the column names of the staging table
        public List<string> GetStagingColumnNames(string DBTableName)
        {
            try
            {
                List<string> output = new();
                db.OpenConnection(ref conn);
                db.cmd.CommandText = $"SELECT StagingColumnName FROM {mappingTable} WHERE DBTableName = '{DBTableName}'";
                db.cmd.CommandType = CommandType.Text;

                reader = db.cmd.ExecuteReader();
                while(reader.Read())
                {
                    output.Add(reader.GetString(0));
                }
                //dt.Load(reader);
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
        #endregion

        #region Get the columns of the SharePoint List
        public List<string> GetSPColumnNames(string DBTableName)
        {
            try
            {
                db.OpenConnection(ref conn);
                db.cmd.CommandText = $"SELECT SPColumnName FROM {mappingTable} WHERE DBTableName = '{DBTableName}'";
                db.cmd.CommandType = CommandType.Text;

                reader = db.cmd.ExecuteReader();
                dt.Load(reader);

                return Utility.ConvertDataTableToList<string>(dt);
            }
            catch (Exception ex)
            {
                db.CloseConnection(ref conn);
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Get tha staging table name
        public string GetStagingTableName(string DBTableName)
        {
            try
            {
                string stagingTable = string.Empty;
                db.OpenConnection(ref conn);
                db.cmd.CommandText = $"SELECT TOP 1 [StagingTableName] FROM {mappingTable} WHERE DBTableName = '{DBTableName}'";
                db.cmd.CommandType = CommandType.Text;

                reader = db.cmd.ExecuteReader();

                while(reader.Read())
                {
                    stagingTable = reader.GetString(0);
                }
                db.CloseConnection(ref conn);
                db.CloseDataReader(reader);
                return stagingTable;

            }
            catch (Exception ex)
            {
                db.CloseConnection(ref conn);
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Get the data from the staging table
        public List<Dictionary<string, object>> GetDataFromStaging(string DBTableName)
        {
            try
            {
                List<string> stagingColumns = GetStagingColumnNames(DBTableName);
                string stagingTableName = GetStagingTableName(DBTableName);
                List<Dictionary<string, object>> queryResults = new();

                db.OpenConnection(ref conn);
                db.cmd.CommandText = $"SELECT DISTINCT {string.Join(", ", stagingColumns)} FROM {stagingTableName}";
                db.cmd.CommandType = CommandType.Text;

                reader = db.cmd.ExecuteReader();
                
                while(reader.Read())
                {
                    Dictionary<string, object> row = new Dictionary<string, object>();
                    foreach(var col in stagingColumns)
                    {
                        row[col] = reader.GetValue(col);
                    }
                    queryResults.Add(row);
                }

                db.CloseConnection(ref conn);
                db.CloseDataReader(reader);
                return queryResults;
                
            }
            catch(Exception ex)
            {
                db.CloseConnection(ref conn);
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Build the query to check the data in the destination table
        public List<string> BuildQuery(string DBTableName)
        {
            try
            {
                List<string> QueryList = new List<string>();
                List<string> DBColumns = GetDBColumnNames(DBTableName);
                List<string> StagingColumns = GetStagingColumnNames(DBTableName);
                List<Dictionary<string, object>> QueryResults = GetDataFromStaging(DBTableName);

                for(int i = 0; i < QueryResults.Count; i++)
                {
                    Dictionary<string, object> row = QueryResults[i];
                    List<string> conditions = new List<string>();

                    for(int j = 0; j < StagingColumns.Count; j++)
                    {
                        string key = StagingColumns[j];
                        object value = row[key];
                        object columnValue;

                        if((value is string) || (value is DateTime) || (value is DateOnly))
                        {
                            columnValue = $"'{value}'";
                            conditions.Add($"{DBColumns[j]} = {columnValue}");
                        }
                        else if((value is decimal) || (value is float))
                        {
                            string? stringValue = value.ToString();
                            stringValue = stringValue.Replace(",", ".");
                            conditions.Add($"{DBColumns[j]} = {stringValue}");
                        }
                        else
                        {
                            conditions.Add($"{DBColumns[j]} = {value}");
                        }
                    }
                    string query = $"SELECT TOP 1 {string.Join(", ", DBColumns)} FROM {DBTableName} WHERE {string.Join(" AND ", conditions)}";
                    QueryList.Add(query);
                }

                return QueryList;
            }
            catch (Exception ex)
            {
                //db.CloseConnection(ref conn);
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Check if the data from the staging already exists in the destination table
        public bool CheckDataExists(string DBTableName)
        {
            try
            {
                bool output;
                List<string> QueryList = BuildQuery(DBTableName);
                db.OpenConnection(ref conn);
                db.cmd.CommandText = QueryList[0];
                db.cmd.CommandType = CommandType.Text;

                object result = db.cmd.ExecuteScalar();
                if(result != null)
                {
                    output = true;
                }
                else
                {
                    output = false;
                }

                return output;
            }
            catch (Exception ex)
            {
                db.CloseConnection(ref conn);
                throw new Exception(ex.Message);
            }

        }
        #endregion

        #region If the data does not exist, then insert it
        public void InsertDataViaSPList(string DBTableName)
        {

        }
        #endregion
        #endregion
    }
}
