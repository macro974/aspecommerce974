using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;


namespace MC3Shopper.Models
{
    public class Database
    {
         public SqlConnection myConnection;
        public static bool isOK = true;
        public Database()
        {
            connection();
        }

        public void connection()
        {
            //this.myConnection = new SqlConnection("Data Source=193.253.99.121,6123;Network Library=DBMSSOCN;Initial Catalog=MC3REUNION;User ID=saa;Password=sa;");
            //this.myConnection = new SqlConnection("Data Source=MC3REUSRV001,6123;Network Library=DBMSSOCN;Initial Catalog=MC3REUNION;User ID=saa;Password=sa;"); // le bon celui la
            this.myConnection = new SqlConnection("Data Source=BRIAN-MSI;Initial Catalog=mc3_sage;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False");
            // 192.168.174.252

            try
            {
                  this.myConnection.Open();
                  Database.isOK = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Database.isOK = false;
            }

            try
            {
                this.myConnection.Close();
            }
            catch (Exception ex)
            { 
            
            }
        }

        public void open()
        {
            try
            {
                this.myConnection.Open();
                Database.isOK = true;
            }
            catch (Exception ex)
            {
                Database.isOK = false;
            }
        }

        public void close()
        {
            try
            {
                this.myConnection.Close();
                Database.isOK = true;
            }
            catch (Exception ex)
            {
                Database.isOK = false;
            }
        }

    }
    
}