using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Windows.Forms;

namespace Makhzan.DataAccessLayer
{

    class Database
        
    { 
        public static String ConnectionString
        {
           
           get
            {
               return ConfigurationManager.ConnectionStrings["Makhzan"].ConnectionString;

            }
        } //instial connection ;
        
        public static List<string> GetAllFacts()
        {
            List<string> Facts = new List<string>();
            using (SqlConnection cond = new SqlConnection(ConnectionString))
            {
                SqlCommand coman = new SqlCommand("SELECT Fact_Name From Factories_T", cond);
                SqlDataReader read = null;
                cond.Open();
                read = coman.ExecuteReader();
                while (read.Read())
                {
                    Facts.Add(read[0].ToString());
                }
                read.Close();
                cond.Close();

            }
            return Facts;
        }

        public static List<Tuple<string,string,string,string,string>> GetStorageProductsForFact(int Fact_ID)
        {
            List<Tuple<string, string,string,string,string>> Products = new List<Tuple<string,string,string,string,string>>();
            using (SqlConnection cond = new SqlConnection(ConnectionString))
            {
                SqlCommand coman = new SqlCommand("SELECT * From  StorageProducts Where Fact_ID=@fctid", cond);
                coman.Parameters.Add(@"fctid", SqlDbType.Int).Value = Fact_ID + 1;
                SqlDataReader read = null;
                cond.Open();
                read = coman.ExecuteReader();
                while (read.Read())
                {
                    Products.Add(new Tuple<string, string, string, string,string>(read[2].ToString(), read[1].ToString(), read[6].ToString(), read[5].ToString(),read[7].ToString()));
                }
                read.Close();
                cond.Close();

            }
            return Products;
        }

        public static void GetUserNames(AutoCompleteStringCollection col)
        {
            using (SqlConnection cond = new SqlConnection(ConnectionString))
            {
                SqlCommand coman = new SqlCommand("SELECT Customer_Name From Customer_T", cond);
                SqlDataReader read = null;
                cond.Open();
                read = coman.ExecuteReader();
                while (read.Read())
                {
                    col.Add(read[0].ToString());
                }
                read.Close();
                cond.Close();
            }
        }

        public static int GetCustomerID_Create (string Name)
        {
            using(SqlConnection Con = new SqlConnection(ConnectionString))
            {
                int ID = 0; 
                SqlCommand com = new SqlCommand("Select Customer_ID From Customer_T Where Customer_Name=@custname", Con);
                com.Parameters.Add(@"custname", SqlDbType.NVarChar).Value = Name;
                Con.Open();
                try
                {
                     ID = (int)com.ExecuteScalar();
                }
                catch
                {
                    ID = 0;
                    com.CommandText = "Insert Into Customer_T Values(@Customername);SELECT IDENT_CURRENT('Customer_T')";
                    com.Parameters.Clear();
                    com.Parameters.Add(@"Customername", SqlDbType.NVarChar).Value = Name;
                    SqlDataReader read = null;
                       read= com.ExecuteReader();
                    while (read.Read())
                    {
                        ID = Convert.ToInt32(read[0]);
                    }
                    
                }
                Con.Close();
                return ID;


            }

        }
        public static int CreateBillForAcustomer(int CustomerID,String BillStat,String PaymentMethod,Double Discount)
        {
            int Bill_ID = 0;
            using (SqlConnection Con = new SqlConnection(ConnectionString))
            {
                SqlCommand com = new SqlCommand("Insert into Bill_T Values (@C_ID,@B_D,@Dis,@B_State,@P_M);SELECT IDENT_CURRENT('Bill_T')", Con);

                com.Parameters.Add(@"C_ID", SqlDbType.Int).Value = CustomerID;
                com.Parameters.Add(@"B_D", SqlDbType.NVarChar).Value = DateTime.Now.ToShortDateString();
                com.Parameters.Add(@"Dis", SqlDbType.Float).Value = Discount;
                com.Parameters.Add(@"B_State", SqlDbType.NVarChar).Value = BillStat;
                com.Parameters.Add(@"P_M", SqlDbType.NVarChar).Value = PaymentMethod;
                Con.Open();
                SqlDataReader read = null;
                read = com.ExecuteReader();
                while(read.Read())
                Bill_ID = int.Parse(read[0].ToString());
                Con.Close();
                return Bill_ID;
            }
        }
        public static void AddProductsToBill(int Bill_ID,int Proudct_ID,float Amont,float PricePerUnit) 
        {
            using (SqlConnection con = new SqlConnection(ConnectionString)) 
            {
                SqlCommand com = new SqlCommand("Insert Into Bill_Products Values (@BID,@PID,@amont,@Pircpunit)",con);
                com.Parameters.Add(@"BID", SqlDbType.Int).Value = Bill_ID;
                com.Parameters.Add(@"PID", SqlDbType.Int).Value = Proudct_ID;
                com.Parameters.Add(@"amont", SqlDbType.Float).Value = Amont;
                com.Parameters.Add(@"Pircpunit", SqlDbType.Float).Value = PricePerUnit;
                con.Open();
                com.ExecuteNonQuery();
                con.Close();  
            }
        }
        public static void InserReminder(int Bill_ID , float ReminderAmont)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand com = new SqlCommand("Insert Into Remainder_T Values (@BID,@amont)", con);
                com.Parameters.Add(@"BID", SqlDbType.Int).Value = Bill_ID;
                com.Parameters.Add(@"amont", SqlDbType.Float).Value = ReminderAmont;
                con.Open();
                com.ExecuteNonQuery();
                con.Close();
            }
        }

        internal static void InserReminder(int bill_ID, double v)
        {
            throw new NotImplementedException();
        }
    }
}
