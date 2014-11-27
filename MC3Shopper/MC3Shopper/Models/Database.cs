using System;
using System.Data.SqlClient;

namespace MC3Shopper.Models
{
    [Serializable]
    public class Database
    {
        public static bool isOK = true;
        public SqlConnection myConnection;

        public Database()
        {
            connection();
        }

        public void connection()
        {
            //this.myConnection = new SqlConnection("Data Source=193.253.99.121,6123;Network Library=DBMSSOCN;Initial Catalog=MC3REUNION;User ID=saa;Password=sa;");
            //this.myConnection = new SqlConnection("Data Source=MC3REUSRV001,6123;Network Library=DBMSSOCN;Initial Catalog=MC3REUNION;User ID=saa;Password=sa;"); // le bon celui la
            myConnection =
                new SqlConnection(
                    "Data Source=BRIAN-MSI;Initial Catalog=mc3_sage;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False");
            // 192.168.174.252

            try
            {
                myConnection.Open();
                isOK = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                isOK = false;
            }

            try
            {
                myConnection.Close();
            }
            catch (Exception ex)
            {
            }
        }

        public void open()
        {
            try
            {
                myConnection.Open();
                isOK = true;
            }
            catch (Exception ex)
            {
                isOK = false;
            }
        }

        public void close()
        {
            try
            {
                myConnection.Close();
                isOK = true;
            }
            catch (Exception ex)
            {
                isOK = false;
            }
        }
    }
}