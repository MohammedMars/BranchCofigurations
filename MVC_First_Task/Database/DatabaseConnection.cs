using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
namespace MVC_First_Task.Data
{
    public class DatabaseConnection
    {

            private const int cSUCCESS = 1;
            private const int cFAIL = 0;
            private const int cEXCEPTION = -1;
            private SqlConnection mConnection;
            private string mConnectionString;

            public DatabaseConnection()
            {
                ConnectionString = GetConnectionString();
                Connection = new SqlConnection(ConnectionString);
            }

            public SqlConnection Connection
            {
                get
                {
                    return mConnection;
                }
                set
                {
                    mConnection = value;
                }
            }

            public string ConnectionString
            {
                get
                {
                    return mConnectionString;
                }
                set
                {
                    mConnectionString = value;
                }
            }
            public void OpenSqlConnection()
            {
                try
                {
                    mConnection.Open();
                    Console.WriteLine("State: " + mConnection.State);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            public void CloseSqlConnection()
            {
                mConnection.Close();
                Console.WriteLine("State: " + mConnection.State);
            }
            static public string GetConnectionString()
            {
                return "Data Source=M-QURAAN;Initial Catalog=sedco;Integrated Security=True";
            }
        }
    }