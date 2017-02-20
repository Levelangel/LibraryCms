using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LibraryCms
{
    public class SqlHelper
    {
        private static SqlConnection _con;

        public static SqlConnection Con
        {
            get
            {
                if (_con == null)
                {
                    _con = new SqlConnection();
                }
                else if (_con.State == ConnectionState.Open || _con.State == ConnectionState.Broken)
                {
                    _con.Close();
                }
                _con.ConnectionString = ConfigurationManager.ConnectionStrings["LibraryCmsDB"].ConnectionString;
                _con.Open();
                return _con;
            }
        }

        public static SqlDataReader GetReader(string sql, params SqlParameter[] pars)
        {
            try
            {
                SqlCommand com = new SqlCommand(sql, Con);
                if (pars.Length > 0)
                {
                    com.Parameters.AddRange(pars);
                }
                SqlDataReader reader = com.ExecuteReader(CommandBehavior.CloseConnection);
                return reader;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable GetDataSet(string sql, params SqlParameter[] pars)
        {
            try
            {
                SqlCommand com = new SqlCommand(sql, Con);
                if (pars.Length > 0)
                {
                    com.Parameters.AddRange(pars);
                }
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(com);
                da.Fill(ds);
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _con.Close();
                _con.Dispose();
            }
        }

        public static DataTable GetDataSet(string sql)
        {
            try
            {
                SqlCommand com = new SqlCommand(sql, Con);
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(com);
                da.Fill(ds);
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _con.Close();
                _con.Dispose();
            }
        }

        public static int ExecuteCommand(string sql, params SqlParameter[] pars)
        {
            int tmp = 1;
            try
            {
                SqlCommand com = new SqlCommand(sql, Con);
                if (pars.Length > 0)
                {
                    com.Parameters.AddRange(pars);
                }
                com.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                tmp = 0;
                throw ex;
            }
            finally
            {
                _con.Close();
                _con.Dispose();
            }
            return tmp;
        }

        public static int ExecuteCommand(string sql)
        {
            int tmp = 1;
            try
            {
                SqlCommand com = new SqlCommand(sql, Con);
                com.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                tmp = 0;
                throw ex;
            }
            finally
            {
                _con.Close();
                _con.Dispose();
            }
            return tmp;
        }
    }
}