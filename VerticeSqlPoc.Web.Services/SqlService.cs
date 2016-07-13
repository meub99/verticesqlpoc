using Dapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using VerticeSqlPoc.Web.Services.Interfaces;
using VerticeSqlPoc.Web.Services.Models;
using VerticeSqlPoc.Web.Services.Models.SQL;
using System.Data;


namespace VerticeSqlPoc.Web.Services
{

    public class SqlService : ISqlService
    {

        #region setup

        private readonly string _connection;
        private IDbConnection connection = null;
        private readonly string connectionString = null;

        public SqlService(string connectionString)
        {
            this.connectionString = connectionString;
            connection =  new SqlConnection(connectionString);
        }

        #endregion

        public IEnumerable<TableGL> FindTop(int count)
        {
            string query = string.Format("SELECT TOP {0} * FROM TableGL ORDER BY Date DESC", count);
            return connection.Query<TableGL>(query);
        }

        public TableGL Find(string id)
        {
            return connection.Query<TableGL>("SELECT * FROM TableGL WHERE UniqueId = @id", new { id = id }).FirstOrDefault();
        }

        //public IEnumerable<CompanyInfo> GetCompanyCodes()
        //{
        //    return connection.Query<CompanyInfo>("SELECT CompanyCode, COUNT(*) AS NumGlItems FROM TableGL Group By CompanyCode");
        //}

        public IEnumerable<CompanyInfo> GetCompanyCodes()
        {
            return connection.Query<CompanyInfo>("SELECT CompanyCode, SUM(NAV) As NavSUM, COUNT(*) AS NumGlItems FROM TableGL2 Group By CompanyCode");
        }

        public IEnumerable<TableGL> GetGLItems(string companyCode)
        {
            return connection.Query<TableGL>("SELECT * FROM TableGL WHERE CompanyCode = @companyCode", new { companyCode = companyCode });
        }

        public IEnumerable<TableGL> GetAllGLItems()
        {
            return connection.Query<TableGL>("SELECT * FROM TableGL");
        }
        public IEnumerable<CompanyNavInfo> GetAllNav()
        {
            string queryString = "SELECT CompanyCode, NAV from TableGL2";

            List<CompanyNavInfo> companyNav = new List<CompanyNavInfo>();

            //SqlService Dapper uses database connection, DataReader expects sql connection
            using (SqlConnection connection =
                new SqlConnection(connectionString))
            {
                // Create the Command and Parameter objects.
                SqlCommand command = new SqlCommand(queryString, connection);

                // Open the connection            
                connection.Open();
                // Create and execute the DataReader, returning the result
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string companyCode = Convert.ToString(reader[0]);
                    double nav = Convert.ToDouble(reader[1]);
                    companyNav.Add(new CompanyNavInfo() { CompanyCode = companyCode, Nav = nav });
                }
                reader.Close();
            }

            return companyNav;
        }
        //public ClientResponse FindAll<T>(T arg) where T: class
        //{

        //    var _response = new ClientResponse();

        //    try
        //    {

        //        var _query = String.Concat("SELECT TOP {0} * FROM ", arg.GetType().Name.ToLower());
        //        _response.Payload.Add("Result", JsonConvert.SerializeObject(SelectList<T>(_query)));
        //        _response.Status = ResponseStatus.Success;


        //    }
        //    catch (Exception ex)
        //    {

        //        _response.Payload.Add("ErrorMessage", ex.Message);
        //        _response.Status = ResponseStatus.Error;
        //        Trace.TraceError(ex.Message);

        //    }

        //    return _response;

        //}


        //List<T> SelectList<T>(string sql, object param = null) where T : class
        //{

        //    using (SqlConnection conn = new SqlConnection(_connection))
        //    {

        //        conn.Open();
        //        return conn.Query<T>(sql, param).ToList();

        //    }

        //}


    }


}
