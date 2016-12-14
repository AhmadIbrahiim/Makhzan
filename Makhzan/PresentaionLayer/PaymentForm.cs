using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Makhzan.DataAccessLayer;
using System.Text.RegularExpressions;
using Makhzan.DataSet;

namespace Makhzan
{
    public partial class PaymentForm : Form
    {
        public PaymentForm()
        {

            InitializeComponent();
            Intial();
            OptimizGroupBox();
        }
        private void Intial()
        {
            Product_Fact_Listview.View = View.Details;
            Product_Fact_Listview.GridLines = true;
            Product_Fact_Listview.FullRowSelect = true;
            //Fatora listview . 
            Fatora_Listview.View = View.Details;
            Fatora_Listview.GridLines = true;
            Fatora_Listview.FullRowSelect = true;
            //get details for combo
            Bill_Listview.View = View.Details;
            Bill_Listview.GridLines = true;
            Bill_Listview.FullRowSelect = true;

            //ReportLstView

            ReportLstView.View = View.Details;
            ReportLstView.GridLines = true;
            ReportLstView.FullRowSelect = true;
            //
            Fact_Combo.DataSource = Database.GetAllFacts();
            Fact_Combo.SelectedIndex = 0;
            PaymentMethodComob.SelectedIndex = 0;
            this.Customer_Name.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            this.Customer_Name.AutoCompleteSource = AutoCompleteSource.CustomSource;
            AutoCompleteStringCollection DataCollection = new AutoCompleteStringCollection();
            Database.GetUserNames(DataCollection);
            Customer_Name.AutoCompleteCustomSource = DataCollection;


   
        }
        public void AddToProudctList(int Index)
        {
            ListViewItem ListviewItem;
            foreach (Tuple<string,string,string,string,string> item in Database.GetStorageProductsForFact(Index))
            {

                ListviewItem = new ListViewItem(new string[] { item.Item1, item.Item2, item.Item3, item.Item4 ,item.Item5});
                Product_Fact_Listview.Items.Add(ListviewItem);
            }
        }
        public void OptimizGroupBox()
        {
            int index =Fact_Combo.SelectedIndex;
            if (index == 0|| index == 2)
            {
                Groupbox5.Text = "كيلو";
            }
            else
            {
                Groupbox5.Text = "شيكارة";

            }
        }
        private void PaymentForm_Load(object sender, EventArgs e)
        {
            Load_BillReport();
        }
        private void Fact_Combo_SelectedIndexChanged(object sender, EventArgs e)
        {
            Product_Fact_Listview.Items.Clear();
            OptimizGroupBox();
            AddToProudctList(Fact_Combo.SelectedIndex);
        }
        private void Product_Fact_Listview_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            ToneTxt.Enabled = true;
            Kilotxtbox.Enabled = true;
            Price_per_tonetxt.Enabled = true;
        }
        Double WhattToPay = 0;
        private void button1_Click(object sender, EventArgs e)
        {

            try
            {
                WhattToPay = double.Parse(WhattoPay.Text);
            }
            catch
            {

            }
          
           
            try
            {
                if (Product_Fact_Listview.FocusedItem.SubItems.Count > 0)
                {
                ListViewItem ListviewItem;
                if (ToneTxt.Text.Length > 0 && Price_per_tonetxt.Text.Length > 0)
                {
                    string Price = Product_Fact_Listview.FocusedItem.SubItems[3].Text;
                    string AvliableAmont = Product_Fact_Listview.FocusedItem.SubItems[2].Text;
                    string ProductName = Product_Fact_Listview.FocusedItem.SubItems[1].Text;
                    float whatShouldHeyPay = float.Parse(Price_per_tonetxt.Text) * float.Parse(ToneTxt.Text);
                        ListviewItem = new ListViewItem(new string[] { ToneTxt.Text + "طن", ProductName, whatShouldHeyPay.ToString() + "جنيه", Product_Fact_Listview.FocusedItem.SubItems[0].Text, ToneTxt.Text,Price_per_tonetxt.Text});
                    Fatora_Listview.Items.Add(ListviewItem);
                    WhattToPay += whatShouldHeyPay;
                }

                    if (Kilotxtbox.Text.Length > 0 && Price_per_tonetxt.Text.Length > 0)
                    {
                        string Price = Product_Fact_Listview.FocusedItem.SubItems[3].Text;
                        string AvliableAmont = Product_Fact_Listview.FocusedItem.SubItems[2].Text;
                        string ProductName = Product_Fact_Listview.FocusedItem.SubItems[1].Text;

                        if (Product_Fact_Listview.FocusedItem.SubItems[4].Text == "1")
                        {
                            float Whatshouldipayforkillo = float.Parse(Price_per_tonetxt.Text) * (float.Parse(Kilotxtbox.Text) / 1000);
                            ListviewItem = new ListViewItem(new string[] { Kilotxtbox.Text + "كيلو", ProductName, Whatshouldipayforkillo.ToString() + "جنيه", Product_Fact_Listview.FocusedItem.SubItems[0].Text, (float.Parse(Kilotxtbox.Text) / 1000).ToString() , Price_per_tonetxt.Text });
                            Fatora_Listview.Items.Add(ListviewItem);
                            WhattToPay += Whatshouldipayforkillo;
                        }
                        else if (Product_Fact_Listview.FocusedItem.SubItems[4].Text == "2")
                        {
                            float Whatshouldipayforkillo = float.Parse(Price_per_tonetxt.Text) * (float.Parse(Kilotxtbox.Text) / 20);
                            ListviewItem = new ListViewItem(new string[] { Kilotxtbox.Text + "شيكاره", ProductName, Whatshouldipayforkillo.ToString() + "جنيه", Product_Fact_Listview.FocusedItem.SubItems[0].Text, (float.Parse(Kilotxtbox.Text) / 20).ToString() , Price_per_tonetxt.Text });
                            Fatora_Listview.Items.Add(ListviewItem);
                            WhattToPay += Whatshouldipayforkillo;


                        }
                        else if (Product_Fact_Listview.FocusedItem.SubItems[4].Text == "3")
                        {

                            float Whatshouldipayforkillo = float.Parse(Price_per_tonetxt.Text) * (float.Parse(Kilotxtbox.Text) / 33);
                            ListviewItem = new ListViewItem(new string[] { Kilotxtbox.Text + "شيكاره", ProductName, Math.Round(Whatshouldipayforkillo, 1) + "جنيه", Product_Fact_Listview.FocusedItem.SubItems[0].Text, (float.Parse(Kilotxtbox.Text) / 33).ToString(),Price_per_tonetxt.Text });
                            Fatora_Listview.Items.Add(ListviewItem);
                            WhattToPay += Whatshouldipayforkillo;


                        }

                    }
                    WhattoPay.Text = Math.Round(WhattToPay, 1).ToString();

                }
                else
                    {
                        MessageBox.Show("الرجاء تحديد منتج واحد لاتمام العمليه ", "خطاء", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
                    }



                
            }
            catch(Exception ex)
            {
                MessageBox.Show("حدث خطاء ! تاكد من ملئ كل البيانات", "خطاء !", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
            }

        }
        private void RemoveFromFatorBtn_Click(object sender, EventArgs e)
        {
            var Price = Regex.Split(Fatora_Listview.FocusedItem.SubItems[2].Text, @"[^0-9\.]+").Where(c => c != "." && c.Trim() != "");
            Fatora_Listview.FocusedItem.Remove();
            WhattoPay.Text = (double.Parse(WhattoPay.Text) - double.Parse(Price.ToArray()[0])).ToString();
                      


        }
        private void DoneFatora_Click(object sender, EventArgs e)
        {
            if (Fatora_Listview.Items.Count > 0 && Customer_Name.Text.Length > 5 && WhattoPay.Text.Length > 0 && WhatHePay_txt.Text.Length > 0)
            {
                BillRepository.GeProducts().Clear();
                DoneFatora.Enabled = false;
                int CustomerID = Database.GetCustomerID_Create(Customer_Name.Text);
                float Reminder = (float.Parse(WhattoPay.Text) - float.Parse(Discout_txt.Text)) - float.Parse(WhatHePay_txt.Text);
                Reminder =(float) Math.Round(Reminder, 1);
                string status = "";
                if (Reminder<=.5)
                {
                    status = "خالص";
                    Reminder = 0;
                }
                else
                {
                    status = "باقي";
                }
              int Bill_ID=  Database.CreateBillForAcustomer(CustomerID, status, PaymentMethodComob.Text, double.Parse(Discout_txt.Text),Convert.ToDouble(WhatHePay_txt.Text),Math.Round(Reminder,1));

                foreach (ListViewItem item in Fatora_Listview.Items)
                {
                    var Price = Regex.Split(item.SubItems[2].Text, @"[^0-9\.]+").Where(c => c != "." && c.Trim() != "");

                    Database.AddProductsToBill(Bill_ID
                        , int.Parse(item.SubItems[3].Text), 
                        float.Parse(item.SubItems[4].Text), float.Parse(Price.ToArray()[0]));



                    BillRepository.GeProducts().Add(new Bill()
                    {
                        Bill_State = status,

                        ProductName = item.SubItems[1].Text,
                        Customer_Name = Customer_Name.Text,
                        Amont = item.SubItems[0].Text,
                        TotalPrice = Double.Parse(Price.ToArray()[0]),
                        Bill_ID = Bill_ID,
                        Discount = double.Parse(Discout_txt.Text), 
                        Pricer_per_Tone = Math.Round(double.Parse(item.SubItems[5].Text), 2), Reminder = Math.Round(Reminder, 2), PaymentMethod = PaymentMethodComob.Text
             , WhatHePay = double.Parse(WhatHePay_txt.Text)
                      
                    });
                }


                Reports.Reports Ob = new Reports.Reports();
                Ob.ChangeLocaSource("Makhzan.Reports.Bill.rdlc");
                Ob.Show();

                DoneFatora.Enabled = true;
               
                   
            }
            else
            {

                MessageBox.Show("تاكد من بعض البيانات !");
            }

            DoneFatora.Enabled = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Bill_Listview.Items.Clear();
            BillRepository.GeProducts().Clear();
            #region GetBillsForCustoemr
            if (RadioName.Checked)
            {
                int UserID = Database.GetCustomerID_Create(SearchInquire.Text);

                ListViewItem ListviewItem;

                List<string[]> Bills = Database.GetBillForCustomer(UserID);
                foreach (string[] item in Bills)
                {

                    ListviewItem = new ListViewItem(item);
                    Bill_Listview.Items.Add(ListviewItem);
                }

            }
            #endregion
            #region SearchWithDate
            if (RadioDate.Checked)
            {

                ListViewItem ListviewItem;

                List<string[]> Bills = Database.GetBillForCustomer(SearchInquire.Text);
                foreach (string[] item in Bills)
                {

                    ListviewItem = new ListViewItem(item);
                    Bill_Listview.Items.Add(ListviewItem);
                }
            }
            #endregion

            #region SearchWithFatorID
            if (RadioBillNumber.Checked)
            {
                ListViewItem ListviewItem;

                List<string[]> Bills = Database.GetBillForCustomer(Convert.ToInt16(SearchInquire.Text),true);
                foreach (string[] item in Bills)
                {

                    ListviewItem = new ListViewItem(item);
                    Bill_Listview.Items.Add(ListviewItem);
                }
            }
            #endregion
        }

        private void RadioName_CheckedChanged(object sender, EventArgs e)
        {
            this.SearchInquire.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            this.SearchInquire.AutoCompleteSource = AutoCompleteSource.CustomSource;
            AutoCompleteStringCollection DataCollection = new AutoCompleteStringCollection();
            Database.GetUserNames(DataCollection);
            SearchInquire.AutoCompleteCustomSource = DataCollection;

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void groupBox7_Enter(object sender, EventArgs e)
        {

        }

        private void Bill_Listview_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Bill_Listview_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int Billid = Convert.ToInt32(Bill_Listview.SelectedItems[0].Text);
            BillRepository.GeProducts().Clear();
            List<string[]> Items = Database.GetProductsForBill(Billid);

            foreach (string[] item in Items)
            {


                BillRepository.GeProducts().Add(new Bill
                {
                    ProductName = item[0],
                    Amont = item[1],
                    Pricer_per_Tone = Convert.ToDouble(item[2]),
                    TotalPrice = Convert.ToDouble(item[3]),
                    Customer_Name = item[4],
                    Bill_State = item[5],
                    WhatHePay =Convert.ToDouble(item[6]),
                    Reminder=Convert.ToDouble(item[7]),
                    PaymentMethod=item[8],
                    Bill_ID= Billid,
                    Discount=Convert.ToDouble(item[9])




                });

            }

            Reports.Reports Ob = new Reports.Reports();
            Ob.ChangeLocaSource("Makhzan.Reports.Bill.rdlc");

            Ob.Show();
        }

        private void tanzeelBtn_Click(object sender, EventArgs e)
        {
            string Stat = Bill_Listview.FocusedItem.SubItems[2].Text;
            double Reminder = Convert.ToDouble(Bill_Listview.FocusedItem.SubItems[6].Text);
            if (Stat == "خالص")
            {
                MessageBox.Show("لا يمكن التنزيل من فتوره خالصه !", "خطاء", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }

            else
            {
                if(Convert.ToDouble(Reminder_txt.Text)>Reminder)
                {

                    MessageBox.Show("لا يمكن تنزل رقم اكبر من الباقي ! .. المتبقي هو : "+Reminder, "خطاء", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
                else if(Convert.ToInt16(Reminder_txt.Text)>0)
                {

                    int Billid = Convert.ToInt32(Bill_Listview.SelectedItems[0].Text);


                    double x = Math.Round(Reminder - Convert.ToDouble(Reminder_txt.Text), 1);
                    Database.InserReminder(Billid, Math.Round(Convert.ToDouble(Reminder_txt.Text), 1), DateTime.Now.ToShortDateString());
                    if (x <= 1)
                    {
                        Database.UpdateBill(Convert.ToInt32(Bill_Listview.FocusedItem.SubItems[0].Text), "خالص", 0,Convert.ToDouble(Reminder_txt.Text) );
                        MessageBox.Show("تم تخليص الفتوره رقم : " + Bill_Listview.FocusedItem.SubItems[0].Text);



                        //
                        BillRepository.GeProducts().Clear();
                        List<string[]> Items = Database.GetProductsForBill(Billid);

                        foreach (string[] item in Items)
                        {


                            BillRepository.GeProducts().Add(new Bill
                            {
                                ProductName = item[0],
                                Amont = item[1],
                                Pricer_per_Tone = Convert.ToDouble(item[2]),
                                TotalPrice = Convert.ToDouble(item[3]),
                                Customer_Name = item[4],
                                Bill_State = item[5]+ " تم التنزيل من الفتوره مبلغ " + Reminder_txt.Text + " بتاريخ: " + DateTime.Now.ToShortDateString(),
                                WhatHePay = Convert.ToDouble(item[6]),
                                Reminder = Convert.ToDouble(item[7]),
                                PaymentMethod = item[8],
                                Bill_ID = Billid,
                                Discount = Convert.ToDouble(item[9])




                            });

                        }

                        Reports.Reports Ob = new Reports.Reports();
                        Ob.ChangeLocaSource("Makhzan.Reports.Bill.rdlc");

                        Ob.Show();



                    }
                    else
                    {
                        Database.UpdateBill(Convert.ToInt32(Bill_Listview.FocusedItem.SubItems[0].Text), "باقي", x, Convert.ToDouble(Reminder_txt.Text) );
                        MessageBox.Show("تم التنزيل من الفتوره رقم  : " + Bill_Listview.FocusedItem.SubItems[0].Text);
                        BillRepository.GeProducts().Clear();
                        List<string[]> Items = Database.GetProductsForBill(Billid);

                        foreach (string[] item in Items)
                        {


                            BillRepository.GeProducts().Add(new Bill
                            {
                                ProductName = item[0],
                                Amont = item[1],
                                Pricer_per_Tone = Convert.ToDouble(item[2]),
                                TotalPrice = Convert.ToDouble(item[3]),
                                Customer_Name = item[4],
                                Bill_State = item[5] + " تم التنزيل من الفتوره مبلغ " + Reminder_txt.Text + " بتاريخ: " + DateTime.Now.ToShortDateString(),
                                WhatHePay = Convert.ToDouble(item[6]),
                                Reminder = Convert.ToDouble(item[7]),
                                PaymentMethod = item[8],
                                Bill_ID = Billid,
                                Discount = Convert.ToDouble(item[9])




                            });

                        }

                        Reports.Reports Ob = new Reports.Reports();
                        Ob.ChangeLocaSource("Makhzan.Reports.Bill.rdlc");

                        Ob.Show();



                    }


                }
                else
                {
                    MessageBox.Show("حدث خطاء ما ! تاكد من كل البيانات ","خطاء ما ", MessageBoxButtons.OK, MessageBoxIcon.Hand);

                }
            }
        }

     
        public void Load_BillReport()
        {

            ListViewItem ListviewItem;
            double TotalEar = 0;
            double Reminder = 0;
           
            List<string[]> Bills = Database.GetBillsOfTheDay(DateTime.Now.ToShortDateString());
            foreach (string[] item in Bills)
            {

                ListviewItem = new ListViewItem(item);
                TotalEar += Math.Round(Convert.ToDouble(item[2]), 1);
                Reminder += Math.Round(Convert.ToDouble(item[4]), 1);

                ReportLstView.Items.Add(ListviewItem);
            }

            List<string[]> Bills_R = Database.GetBillsOfTheDay_Reminder(DateTime.Now.ToShortDateString());
            foreach (string[] item in Bills_R)
            {

                ListviewItem = new ListViewItem(item);
                TotalEar += Math.Round(Convert.ToDouble(item[2]), 1);
                ReportLstView.Items.Add(ListviewItem).ForeColor = Color.Red; ;
            }

            TotalEarn.Text = TotalEar.ToString();
            TotralReminder.Text = Reminder.ToString();
            TotalBills.Text = ReportLstView.Items.Count.ToString();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            BillDailyReposiotry.GeProducts().Clear();
            
            
            foreach (ListViewItem item in ReportLstView.Items)
            {

                BillDailyReposiotry.GeProducts().Add(new BillDaily
                {
                    Bill_ID = Convert.ToInt32(item.SubItems[0].Text),
                    Cust_Name = item.SubItems[1].Text.ToString(),
                    What_He_Pay = item.SubItems[2].Text.ToString(),
                    TotalPriceForBill = item.SubItems[3].Text.ToString(),
                    reminder = item.SubItems[4].Text.ToString(),
                    TotalReminder = TotralReminder.Text,
                    TotalEarn=TotalEarn.Text,
                    totalBills=TotalBills.Text
                    




                });

            }



            DailyReprot Ob = new DailyReprot();
            Ob.Show();
        }
    }
    }

