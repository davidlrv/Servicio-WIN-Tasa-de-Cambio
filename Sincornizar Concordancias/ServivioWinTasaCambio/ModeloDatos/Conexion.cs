using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace SincronizarConcordancia.ModeloDatos
{
    public class Conexion
    {
        private SqlDataReader m_dr;
        private SqlDataAdapter m_da;
        

        private string m_StringConexion;
        private SqlConnection m_cnn;
        private SqlCommand cmd;

        public Conexion()
        {
            try
            {
                //m_StringConexion = ConfigurationSettings.AppSettings["ConexionString"]; 
                m_StringConexion = System.Configuration.ConfigurationManager.ConnectionStrings["NameConexion"].ConnectionString;
                m_cnn = new SqlConnection(m_StringConexion);
            }
            catch (Exception e)
            {
                
                throw;
            }
            
        }
        public dynamic EjecutarProcedimiento(string namepr, string parametro)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();
            
            try
            {
                if (Cnn.State == System.Data.ConnectionState.Open)
                {
                    cmd = new SqlCommand(namepr, m_cnn);
                    cmd.Parameters.Add(new SqlParameter("@Parametros", parametro));
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    //dataGridView1.DataSource = dt;
                }
                else
                {
                    m_cnn.Open();
                    cmd = new SqlCommand(namepr, m_cnn);
                    cmd.Parameters.Add(new SqlParameter("@Parametros", parametro));
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);                    

                }
            }
            catch (Exception x)
            {
                return x;
            }
            finally
            {
                cmd.Dispose();
                m_cnn.Close();
                
            }
            return dt;
        }
        public SqlDataAdapter Da
        {
            get { return m_da; }
            set { m_da = value; }
        }
        public SqlDataReader Dr
        {
            get { return m_dr; }
            set { m_dr = value; }
        }
        public void Cerrar()
        { 
            Dr.Close();
            m_cnn.Close();
        }

        public SqlConnection Cnn
        {
            get { return m_cnn; }
            set { m_cnn = value; }
        }



    }
}
