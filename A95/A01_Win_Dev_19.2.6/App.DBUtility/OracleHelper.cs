using System;
using System.Collections.Generic;
using System.Text;
using System.Data.OracleClient;
using System.Configuration;
using System.Data;

namespace App.DBUtility
{
    /// <summary>
    /// SqlServer database operations class.
    /// </summary>
    public partial class OracleHelper:HelperBase 
    {
        string connection_str;
        public OracleHelper()
        {
            
            connection_str = default_connection_str;
            Connection = new OracleConnection(connection_str);
            Command = Connection.CreateCommand();
        }

        public OracleHelper(int ConnectionStringsIndex)
        {
            
            connection_str = ConfigurationManager.ConnectionStrings[ConnectionStringsIndex].ConnectionString;
            Connection = new OracleConnection(connection_str);
            Command = Connection.CreateCommand();
        }

        public OracleHelper(string ConnectionString)
        {
            
            connection_str = ConnectionString;
            Connection = new OracleConnection(connection_str);
            Command = Connection.CreateCommand();
        }

        public override void Open()
        {
            base.Open();
        }

        public OracleParameter AddParameter(string ParameterName, OracleType type, object value)
        {
            return AddParameter(ParameterName, type, value, ParameterDirection.Input);
        }

        public OracleParameter AddParameter(string ParameterName, OracleType type, object value, ParameterDirection direction)
        {
            OracleParameter param = new OracleParameter(ParameterName, type);
            param.Value = value;
            param.Direction = direction;
            Command.Parameters.Add(param);
            return param;
        }

        public OracleParameter AddParameter(string ParameterName, OracleType type, int size, object value)
        {
            return AddParameter(ParameterName, type, size, value, ParameterDirection.Input);
        }

        public OracleParameter AddParameter(string ParameterName, OracleType type, int size, object value, ParameterDirection direction)
        {
            OracleParameter param = new OracleParameter(ParameterName, type, size);
            param.Direction = direction;
            param.Value = value;
            Command.Parameters.Add(param);
            return param;
        }

        public void AddRangeParameters(OracleParameter[] parameters)
        {
            Command.Parameters.AddRange(parameters);
        }
    }
}
