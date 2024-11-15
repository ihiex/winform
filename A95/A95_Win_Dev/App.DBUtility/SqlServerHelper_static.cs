﻿/*
  Source description:
     This part of the source and the source network, and modify non-DBHelper.org site open source, source code using static methods in this section, you can easily perform some simple Sql statement.
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Windows.Forms;

namespace App.DBUtility
{
    public partial class SqlServerHelper
    {
        public static readonly string default_connection_str = ConfigurationManager.ConnectionStrings["SqlServerHelper"].ConnectionString;

        private static Hashtable parmCache = Hashtable.Synchronized(new Hashtable());

        public static int ExecuteNonQuery(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                int val = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                return val;
            }
        }

        public static int ExecuteNonQuery(CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            return ExecuteNonQuery(default_connection_str, cmdType, cmdText, commandParameters);
        }

        public static int ExecuteNonQuery(string cmdText, params SqlParameter[] commandParameters)
        {
            return ExecuteNonQuery(default_connection_str, CommandType.Text, cmdText, commandParameters);
        }

        public static int ExecuteNonQueryProc(string StoredProcedureName, params SqlParameter[] commandParameters)
        {
            return ExecuteNonQuery(default_connection_str, CommandType.StoredProcedure, StoredProcedureName, commandParameters);
        }

        public static int ExecuteNonQuery(SqlConnection connection, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return val;
        }

        public static int ExecuteNonQuery(SqlTransaction trans, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, commandParameters);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return val;
        }

        public static SqlDataReader ExecuteReader(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            SqlConnection conn = new SqlConnection(connectionString);
            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return rdr;
            }
            catch
            {
                conn.Close();
                throw;
            }
        }

        public static SqlDataReader ExecuteReader(SqlConnection conn, string cmdText, params SqlParameter[] commandParameters)
        {
            return ExecuteReader(default_connection_str, CommandType.Text, cmdText, commandParameters);
        }

        public static SqlDataReader ExecuteReader(string cmdText, params SqlParameter[] commandParameters)
        {
            return ExecuteReader(default_connection_str, CommandType.Text, cmdText, commandParameters);
        }

        public static SqlDataReader ExecuteReaderProc(string StoredProcedureName, params SqlParameter[] commandParameters)
        {
            return ExecuteReader(default_connection_str, CommandType.StoredProcedure, StoredProcedureName, commandParameters);
        }

        public static SqlDataReader ExecuteReader(CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            return ExecuteReader(default_connection_str, cmdType, cmdText, commandParameters);
        }

        public static object ExecuteScalar(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
                object val = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                return val;
            }
        }

        public static object ExecuteScalar(string cmdText, params SqlParameter[] commandParameters)
        {
            return ExecuteScalar(default_connection_str, CommandType.Text, cmdText, commandParameters);
        }

        public static object ExecuteScalarProc(string StoredProcedureName, params SqlParameter[] commandParameters)
        {
            return ExecuteScalar(default_connection_str, CommandType.StoredProcedure, StoredProcedureName, commandParameters);
        }

        public static object ExecuteScalar(CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            return ExecuteScalar(default_connection_str, cmdType, cmdText, commandParameters);
        }

        public static object ExecuteScalar(SqlConnection connection, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
            object val = cmd.ExecuteScalar();
            cmd.Parameters.Clear();
            return val;
        }

        public static void CacheParameters(string cacheKey, params SqlParameter[] commandParameters)
        {
            parmCache[cacheKey] = commandParameters;
        }

        public static SqlParameter[] GetCachedParameters(string cacheKey)
        {
            SqlParameter[] cachedParms = (SqlParameter[])parmCache[cacheKey];
            if (cachedParms == null)
                return null;
            SqlParameter[] clonedParms = new SqlParameter[cachedParms.Length];
            for (int i = 0, j = cachedParms.Length; i < j; i++)
                clonedParms[i] = (SqlParameter)((ICloneable)cachedParms[i]).Clone();
            return clonedParms;
        }

        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, CommandType cmdType, string cmdText, SqlParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = cmdType;
            if (cmdParms != null)
            {
                foreach (SqlParameter parm in cmdParms)
                    cmd.Parameters.Add(parm);
            }
        }

        public static DataTable ReadTable(SqlTransaction transaction, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            PrepareCommand(cmd, transaction.Connection, transaction, cmdType, cmdText, commandParameters);
            DataTable dt = HelperBase.ReadTable(cmd);
            cmd.Parameters.Clear();
            return dt;
        }

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(default_connection_str);
        }

        public static DataTable ReadTable(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return ReadTable(connection, cmdType, cmdText, commandParameters);
            }
        }
        public static DataTable ReadTable(CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            return ReadTable(default_connection_str, cmdType, cmdText, commandParameters);
        }

        public static DataTable ReadTable(SqlConnection connection, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
            DataTable dt = HelperBase.ReadTable(cmd);
            cmd.Parameters.Clear();
            return dt;
        }

        public static SqlParameter CreateInputParameter(string paramName, SqlDbType dbtype, object value)
        {
            return CreateParameter(ParameterDirection.Input, paramName, dbtype, 0, value);
        }
        public static SqlParameter CreateInputParameter(string paramName, SqlDbType dbtype, int size, object value)
        {
            return CreateParameter(ParameterDirection.Input, paramName, dbtype, size, value);
        }

        public static SqlParameter CreateOutputParameter(string paramName, SqlDbType dbtype)
        {
            return CreateParameter(ParameterDirection.Output, paramName, dbtype, 0, DBNull.Value);
        }

        public static SqlParameter CreateOutputParameter(string paramName, SqlDbType dbtype, int size)
        {
            return CreateParameter(ParameterDirection.Output, paramName, dbtype, size, DBNull.Value);
        }

        public static SqlParameter CreateParameter(ParameterDirection direction, string paramName, SqlDbType dbtype, int size, object value)
        {
            SqlParameter param = new SqlParameter(paramName, dbtype, size);
            param.Value = value;
            param.Direction = direction;
            return param;
        }


        public static DataTable Data_Table(string S_Sql)
        {
            DataTable DT = new DataTable();
            try
            {
                SqlConnection Conn = new SqlConnection(default_connection_str);
                Conn.Open();
                SqlCommand cmd = new SqlCommand(S_Sql, Conn);
                SqlDataAdapter Sql_DA = new SqlDataAdapter(cmd);
                Sql_DA.Fill(DT);
                Sql_DA.Dispose();
                Conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return DT;
        }

        public static DataTable ExecuteDataTable(string S_Sql)
        {
            DataTable DT = new DataTable();
            try
            {
                SqlConnection Conn = new SqlConnection(default_connection_str);
                Conn.Open();
                SqlCommand cmd = new SqlCommand(S_Sql, Conn);
                SqlDataAdapter Sql_DA = new SqlDataAdapter(cmd);
                Sql_DA.Fill(DT);
                Sql_DA.Dispose();
                Conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return DT;
        }

        public static DataTable ExecuteDataTable(string cmdText, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(default_connection_str))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = cmdText;
                    cmd.Parameters.AddRange(parameters);
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        conn.Close();
                        return dt;
                    }

                }
            }
        }

        public static SqlDataReader ExecuteDataReader(string cmdText, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(default_connection_str))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = cmdText;
                    cmd.Parameters.AddRange(parameters);
                    conn.Close();
                    return cmd.ExecuteReader(CommandBehavior.CloseConnection);
                }
            }
        }



        public static DataSet Data_Set(string S_Sql)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlConnection Conn = new SqlConnection(default_connection_str);
                Conn.Open();
                SqlCommand cmd = new SqlCommand(S_Sql, Conn);
                SqlDataAdapter Sql_DA = new SqlDataAdapter(cmd);
                Sql_DA.Fill(ds);
                Conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return ds;
        }


        public static string ExecSql(string S_Sql)
        {
            string S_Result = "OK";
            try
            {
                SqlConnection Conn = new SqlConnection(default_connection_str);
                Conn.Open();
                SqlCommand cmd = new SqlCommand(S_Sql, Conn);
                cmd.ExecuteNonQuery();
                Conn.Close();
                cmd.Dispose();
            }
            catch (Exception ex)
            {
                S_Result = ex.Message;
            }
            return S_Result;
        }

        public static DataSet ExecuteNonQueryPro(string ProName, params SqlParameter[] commandParameters)
        {
            DataSet daset = new DataSet();
            try
            {
                using (SqlConnection con = new SqlConnection(default_connection_str))
                {
                    con.Open();
                    SqlCommand com = new SqlCommand(ProName, con);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddRange(commandParameters);
                    SqlDataAdapter sqlData = new SqlDataAdapter();
                    sqlData.SelectCommand = com;
                    sqlData.Fill(daset);
                    return daset;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }

}
