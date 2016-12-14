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
        public static List<Tuple<string, string, string, string, string>> GetStorageProductsForFact(int Fact_ID)
        {
            List<Tuple<string, string, string, string, string>> Products = new List<Tuple<string, string, string, string, string>>();
            using (SqlConnection cond = new SqlConnection(ConnectionString))
            {
                SqlCommand coman = new SqlCommand("SELECT * From  StorageProducts Where Fact_ID=@fctid", cond);
                coman.Parameters.Add(@"fctid", SqlDbType.Int).Value = Fact_ID + 1;
                SqlDataReader read = null;
                cond.Open();
                read = coman.ExecuteReader();
                while (read.Read())
                {
                    Products.Add(new Tuple<string, string, string, string, string>(read[2].ToString(), read[1].ToString(), read[6].ToString(), read[5].ToString(), read[7].ToString()));
                }
                read.Close();
                cond.Close();

            }
            return Products;
        }
        public static string[] Product_For_Fact(int Fact_ID)
        {
            List<string> Product = new List<string>();
            using (SqlConnection cond = new SqlConnection(ConnectionString))
            {
                SqlCommand coman = new SqlCommand("SELECT Product_Name From  StorageProducts Where Fact_ID=@fctid", cond);
                coman.Parameters.Add(@"fctid", SqlDbType.Int).Value = Fact_ID + 1;
                SqlDataReader read = null;
                cond.Open();
                read = coman.ExecuteReader();
                while (read.Read())
                {
                    Product.Add(read[0].ToString());
                }
                read.Close();
                cond.Close();

            }

            return Product.ToArray();
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
        } // this is for auto complete 
        public static int GetCustomerID_Create(string Name)
        {
            using (SqlConnection Con = new SqlConnection(ConnectionString))
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
                    read = com.ExecuteReader();
                    while (read.Read())
                    {
                        ID = Convert.ToInt32(read[0]);
                    }

                }
                Con.Close();
                return ID;


            }

        }
        public static int CreateBillForAcustomer(int CustomerID, String BillStat, String PaymentMethod, Double Discount, double WhatHePy, double remind)
        {
            int Bill_ID = 0;
            using (SqlConnection Con = new SqlConnection(ConnectionString))
            {
                SqlCommand com = new SqlCommand("Insert into Bill_T Values (@C_ID,@B_D,@Dis,@B_State,@P_M,@WHP,@Rmind);SELECT IDENT_CURRENT('Bill_T')", Con);

                com.Parameters.Add(@"C_ID", SqlDbType.Int).Value = CustomerID;
                com.Parameters.Add(@"B_D", SqlDbType.NVarChar).Value = DateTime.Now.ToShortDateString();
                com.Parameters.Add(@"Dis", SqlDbType.Float).Value = Math.Round(Discount, 1);
                com.Parameters.Add(@"B_State", SqlDbType.NVarChar).Value = BillStat;
                com.Parameters.Add(@"P_M", SqlDbType.NVarChar).Value = PaymentMethod;
                com.Parameters.Add(@"WHP", SqlDbType.Float).Value = WhatHePy;
                com.Parameters.Add(@"Rmind", SqlDbType.Float).Value = remind;



                Con.Open();
                SqlDataReader read = null;
                read = com.ExecuteReader();
                while (read.Read())
                    Bill_ID = int.Parse(read[0].ToString());
                Con.Close();
                return Bill_ID;
            }
        }
        public static void AddProductsToBill(int Bill_ID, int Proudct_ID, float Amont, float PricePerUnit)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand com = new SqlCommand("Insert Into Bill_Products Values (@BID,@PID,@amont,@Pircpunit)", con);
                com.Parameters.Add(@"BID", SqlDbType.Int).Value = Bill_ID;
                com.Parameters.Add(@"PID", SqlDbType.Int).Value = Proudct_ID;
                com.Parameters.Add(@"amont", SqlDbType.Float).Value = Math.Round(Amont, 1);
                com.Parameters.Add(@"Pircpunit", SqlDbType.Float).Value = Math.Round(PricePerUnit, 1);
                con.Open();
                com.ExecuteNonQuery();
                con.Close();
            }
        }
        public static void InserReminder(int Bill_ID, double ReminderAmont, string Date)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand com = new SqlCommand("Insert Into Remainder_T Values (@BID,@amont,@Dt)", con);
                com.Parameters.Add(@"BID", SqlDbType.Int).Value = Bill_ID;
                com.Parameters.Add(@"@Dt", SqlDbType.NVarChar).Value = Date;
                com.Parameters.Add(@"amont", SqlDbType.Float).Value = Math.Round(ReminderAmont, 1);
                con.Open();
                com.ExecuteNonQuery();
                con.Close();
            }
        }
        public static List<string[]> GetBillForCustomer(int Cust_ID)
        {
            List<string[]> Bill = new List<string[]>();
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                SqlCommand Com = new SqlCommand("Select Bill_T.Bill_ID,Bill_T.Bill_Date,Bill_T.Bill_State,Bill_T.Payment_Method,Bill_T.Discount,Bill_T.What_He_Pay,Bill_T.Reminder,Customer_T.Customer_Name From Bill_T,Bill_Products,Customer_T where Bill_T.Bill_ID=Bill_Products.Bill_ID and Customer_T.Customer_ID=Bill_T.Customer_ID and Bill_T.Customer_ID=@id GROUP BY Bill_T.Bill_ID,Bill_T.Bill_State,Bill_Date,Payment_Method,Discount,Customer_Name,Bill_T.What_He_Pay,Bill_T.Reminder", con);
                Com.Parameters.Add(@"id", SqlDbType.Int).Value = Cust_ID;
                SqlDataReader read = null;
                read = Com.ExecuteReader();
                while (read.Read())
                {
                    string[] Data = new string[9];
                    Data[0] = read[0].ToString();
                    Data[1] = read[1].ToString();
                    Data[2] = read[2].ToString();
                    Data[3] = read[3].ToString();
                    Data[4] = read[4].ToString();
                    Data[5] = read[5].ToString();
                    Data[6] = read[6].ToString();
                    Data[7] = read[7].ToString();
                    Bill.Add(Data);
                }

                con.Close();
                return Bill;

            }
        }
        public static List<string[]> GetBillForCustomer(string Date)
        {
            List<string[]> Bill = new List<string[]>();
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                SqlCommand Com = new SqlCommand("Select Bill_T.Bill_ID,Bill_T.Bill_Date,Bill_T.Bill_State,Bill_T.Payment_Method,Bill_T.Discount,Bill_T.What_He_Pay,Bill_T.Reminder,Customer_T.Customer_Name From Bill_T,Bill_Products,Customer_T where Bill_T.Bill_ID=Bill_Products.Bill_ID and Customer_T.Customer_ID=Bill_T.Customer_ID and Bill_T.Bill_Date=@Date GROUP BY Bill_T.Bill_ID,Bill_T.Bill_State,Bill_Date,Payment_Method,Discount,Customer_Name,Bill_T.What_He_Pay,Bill_T.Reminder", con);
                Com.Parameters.Add(@"Date", SqlDbType.NVarChar).Value = Date;
                SqlDataReader read = null;
                read = Com.ExecuteReader();
                while (read.Read())
                {
                    string[] Data = new string[9];
                    Data[0] = read[0].ToString();
                    Data[1] = read[1].ToString();
                    Data[2] = read[2].ToString();
                    Data[3] = read[3].ToString();
                    Data[4] = read[4].ToString();
                    Data[5] = read[5].ToString();
                    Data[6] = read[6].ToString();
                    Data[7] = read[7].ToString();
                    Bill.Add(Data);
                }

                con.Close();
                return Bill;

            }
        }
        public static List<string[]> GetBillsOfTheDay(string Date)
        {
            List<string[]> Bill = new List<string[]>();
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                SqlCommand Com = new SqlCommand("Select Bill_T.Bill_ID,Customer_T.Customer_Name,Bill_T.What_He_Pay,Sum(Bill_Products.Price_Per_Unit),Bill_T.Reminder From Bill_T,Bill_Products,Customer_T where Bill_T.Bill_ID=Bill_Products.Bill_ID and Customer_T.Customer_ID=Bill_T.Customer_ID and Bill_T.Bill_Date=@bd GROUP BY Bill_T.Bill_ID,Bill_T.Bill_State,Bill_Date,Payment_Method,Discount,Customer_Name,Bill_T.What_He_Pay,Bill_T.Reminder", con);
                Com.Parameters.Add(@"bd", SqlDbType.NVarChar).Value = Date;
                SqlDataReader read = null;
                read = Com.ExecuteReader();
                while (read.Read())
                {
                    string[] Data = new string[5];
                    Data[0] = read[0].ToString();
                    Data[1] = read[1].ToString();
                    Data[2] = read[2].ToString();
                    Data[3] = read[3].ToString();
                    Data[4] = read[4].ToString();
                    Bill.Add(Data);
                }

                con.Close();
                return Bill;

            }
        }
        public static List<string[]> GetBillsOfTheDay_Reminder(string Date)
        {
            List<string[]> Bill = new List<string[]>();
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                SqlCommand Com = new SqlCommand("Select Bill_T.Bill_ID,Customer_T.Customer_Name,Remainder_T.Remainder_Amont,Bill_T.Reminder From Bill_T,Bill_Products,Customer_T,Remainder_T where Bill_T.Bill_ID=Bill_Products.Bill_ID and Remainder_T.Bill_ID=Bill_T.Bill_ID And Customer_T.Customer_ID=Bill_T.Customer_ID and Bill_T.Bill_Date=@bd GROUP BY Bill_T.Bill_ID,Bill_T.Bill_State,Bill_Date,Payment_Method,Discount,Customer_Name,Bill_T.What_He_Pay,Bill_T.Reminder,Remainder_T.Remainder_Amont", con);
                Com.Parameters.Add(@"bd", SqlDbType.NVarChar).Value = Date;
                SqlDataReader read = null;
                read = Com.ExecuteReader();
                while (read.Read())
                {
                    string[] Data = new string[5];
                    Data[0] = read[0].ToString();
                    Data[1] = read[1].ToString();
                    Data[2] = read[2].ToString();
                    Data[3] = "تنزيل";
                    Data[4] = read[3].ToString() + "بعدالتنزيل";
                    Bill.Add(Data);
                }

                con.Close();
                return Bill;

            }
        }
        public static void UpdateBill(int BillID, String Sats, double Reminder, double whathepay)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand com = new SqlCommand("Update Bill_T Set What_He_Pay+=@wht ,Bill_State=@st,Reminder=@rm where Bill_ID=@id", con);
                con.Open();
                com.Parameters.Add(@"st", SqlDbType.NVarChar).Value = Sats;
                com.Parameters.Add(@"rm", SqlDbType.Float).Value = Reminder;
                com.Parameters.Add(@"wht", SqlDbType.Float).Value = whathepay;
                com.Parameters.Add(@"id", SqlDbType.Int).Value = BillID;
                com.ExecuteNonQuery();
                con.Close();

            }
        }
        public static List<string[]> GetBillForCustomer(int BillID, bool isBill)
        {
            List<string[]> Bill = new List<string[]>();
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                SqlCommand Com = new SqlCommand("Select Bill_T.Bill_ID,Bill_T.Bill_Date,Bill_T.Bill_State,Bill_T.Payment_Method,Bill_T.Discount,Bill_T.What_He_Pay,Bill_T.Reminder,Customer_T.Customer_Name From Bill_T,Bill_Products,Customer_T where Bill_T.Bill_ID=Bill_Products.Bill_ID and Customer_T.Customer_ID=Bill_T.Customer_ID and Bill_T.Bill_ID=@Date GROUP BY Bill_T.Bill_ID,Bill_T.Bill_State,Bill_Date,Payment_Method,Discount,Customer_Name,Bill_T.What_He_Pay,Bill_T.Reminder", con);
                Com.Parameters.Add(@"Date", SqlDbType.Int).Value = BillID;
                SqlDataReader read = null;
                read = Com.ExecuteReader();
                while (read.Read())
                {
                    string[] Data = new string[9];
                    Data[0] = read[0].ToString();
                    Data[1] = read[1].ToString();
                    Data[2] = read[2].ToString();
                    Data[3] = read[3].ToString();
                    Data[4] = read[4].ToString();
                    Data[5] = read[5].ToString();
                    Data[6] = read[6].ToString();
                    Data[7] = read[7].ToString();
                    Bill.Add(Data);
                }

                con.Close();
                return Bill;

            }
        }
        public static List<string[]> GetProductsForBill(int BillID)
        {
            List<string[]> BillDetails = new List<string[]>();
            using (SqlConnection Con = new SqlConnection(ConnectionString))
            {
                SqlCommand Com = new SqlCommand("select Product_T.Product_Name,Product_T.Product_Type,Bill_Products.Amount,Bill_Products.Price_Per_Unit,Customer_T.Customer_Name,Bill_T.Bill_State,Bill_T.What_He_Pay,Bill_T.Reminder,Bill_T.Payment_Method,Bill_T.Discount From Product_T,Bill_Products,Customer_T,Bill_T Where Bill_T.Bill_ID=Bill_Products.Bill_ID And Bill_T.Customer_ID=Customer_T.Customer_ID And Bill_Products.Product_ID=Product_T.Product_ID AND Bill_T.Bill_ID=@Bid", Con);
                Con.Open();
                Com.Parameters.Add(@"Bid", SqlDbType.Int).Value = BillID;

                SqlDataReader read = null;
                read = Com.ExecuteReader();
                while (read.Read())
                {
                    string[] Data = new string[10];
                    Data[0] = read[0].ToString();
                    if (read[1].ToString() == "1")
                    {
                        if (Convert.ToDouble(read[2]) >= 1)
                        {
                            Data[1] = "طن" + read[2].ToString();
                            Data[2] = (Convert.ToInt32(read[3]) / Convert.ToInt32(read[2])).ToString();
                            Data[3] = read[3].ToString();

                        }
                        else
                        {
                            Data[1] = "كيلو" + Math.Round(Convert.ToDouble(read[2]) * 1000, 0);
                            Data[2] = Math.Round((Convert.ToDouble(read[3]) / Convert.ToDouble(read[2])), 0).ToString();
                            Data[3] = read[3].ToString();

                        }
                    }

                    if (read[1].ToString() == "2")
                    {
                        if (Convert.ToDouble(read[2]) >= 1)
                        {
                            Data[1] = "طن" + read[2].ToString();
                            Data[2] = (Convert.ToInt32(read[3]) / Convert.ToInt32(read[2])).ToString();
                            Data[3] = read[3].ToString();


                        }
                        else
                        {
                            Data[1] = "شيكاره" + Math.Round(Convert.ToDouble(read[2]) * 20, 0);
                            Data[2] = Math.Round((Convert.ToDouble(read[3]) / Convert.ToDouble(read[2])), 0) + "";
                            Data[3] = read[3].ToString();

                        }
                    }
                    if (read[1].ToString() == "3")
                    {
                        if (Convert.ToDouble(read[2]) >= 1)
                        {
                            Data[1] = "طن" + read[2].ToString();
                            Data[2] = (Convert.ToInt32(read[3]) / Convert.ToInt32(read[2])).ToString();
                            Data[3] = read[3].ToString();

                        }
                        else
                        {
                            double x = Convert.ToDouble(read[2]) * 33;
                            Data[1] = "شيكاره" + Math.Round(x, 0);
                            Data[2] = Math.Round((Convert.ToDouble(read[3]) / Convert.ToDouble(read[2])), 0).ToString();
                            Data[3] = read[3].ToString();

                        }
                    }

                    Data[4] = read[4].ToString();
                    Data[5] = read[5].ToString();
                    Data[6] = read[6].ToString();

                    Data[7] = read[7].ToString();
                    Data[8] = read[8].ToString();
                    Data[9] = read[9].ToString();
                    BillDetails.Add(Data);

                }

                return BillDetails;
            }


        }

        public static void InsertCouponForFact(int amont ,int Fact)
        {
            using(SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand com = new SqlCommand("Insert Into Coupon_T Values (@fid,@cm,@date)", con);
                com.Parameters.Add(@"fid", SqlDbType.Int).Value = Fact;
                com.Parameters.Add(@"cm", SqlDbType.Int).Value = amont;
                com.Parameters.Add(@"date", SqlDbType.Int).Value = DateTime.Now.ToShortDateString();
                con.Open();
                com.ExecuteNonQuery();
                con.Close();



            }
        }
        public static int[] GetCouponsAmontForFact(int fact)
        {
            int[] Amoun = new int[2];

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand Com = new SqlCommand("Select Coupon_T.Coupon_Amount,Coupon_ID From Coupon_T Where Coupon_T.Fact_ID=@id", con);
                Com.Parameters.Add(@"id", SqlDbType.Int).Value = fact;

                SqlDataReader read = null;
                con.Open();
                read = Com.ExecuteReader();
                while (read.Read())
                {
                    Amoun[0] = Convert.ToInt32(read[0]);
                    Amoun[1] = Convert.ToInt32(read[1]);
                }
                con.Close();

            }
            return Amoun;
            }
        public static void InsertCouponDeatails (int Product_ID,int Amont,int Price_Per_unit,int couponID)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand com = new SqlCommand("Insert into Coupon_Details Values (@ci,@pid,@pamount,@ppu)", con);
                com.Parameters.Add(@"ci", SqlDbType.Int).Value = couponID;
                com.Parameters.Add(@"pid", SqlDbType.Int).Value = Product_ID;
                com.Parameters.Add(@"pamount", SqlDbType.Int).Value = Amont;
                com.Parameters.Add(@"ppu", SqlDbType.Int).Value = Price_Per_unit;
                con.Open();
                com.ExecuteNonQuery();
                con.Close();
                  
            }

            UpdateAmontPrice(couponID, Amont);
        }
        private static  void UpdateAmontPrice(int CouponID,int NewAmont)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand com = new SqlCommand("Update Coupon_T set Coupon_Amount-=@am Where Coupon_ID=@cp", con);
                com.Parameters.Add(@"cp", SqlDbType.Int).Value = CouponID;
                com.Parameters.Add(@"am", SqlDbType.Int).Value = NewAmont;
        
                con.Open();
                com.ExecuteNonQuery();
                con.Close();

            }
        }
     
    
    }
}
