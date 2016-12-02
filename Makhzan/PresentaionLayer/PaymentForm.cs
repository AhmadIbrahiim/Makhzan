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
                        ListviewItem = new ListViewItem(new string[] { ToneTxt.Text + "طن", ProductName, whatShouldHeyPay.ToString() + "جنيه", Product_Fact_Listview.FocusedItem.SubItems[0].Text, ToneTxt.Text});
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
                            ListviewItem = new ListViewItem(new string[] { Kilotxtbox.Text + "كيلو", ProductName, Whatshouldipayforkillo.ToString() + "جنيه", Product_Fact_Listview.FocusedItem.SubItems[0].Text, (float.Parse(Kilotxtbox.Text) / 1000).ToString() });
                            Fatora_Listview.Items.Add(ListviewItem);
                            WhattToPay += Whatshouldipayforkillo;
                        }
                        else if (Product_Fact_Listview.FocusedItem.SubItems[4].Text == "2")
                        {
                            float Whatshouldipayforkillo = float.Parse(Price_per_tonetxt.Text) * (float.Parse(Kilotxtbox.Text) / 20);
                            ListviewItem = new ListViewItem(new string[] { Kilotxtbox.Text + "شيكاره", ProductName, Whatshouldipayforkillo.ToString() + "جنيه", Product_Fact_Listview.FocusedItem.SubItems[0].Text, (float.Parse(Kilotxtbox.Text) / 20).ToString() });
                            Fatora_Listview.Items.Add(ListviewItem);
                            WhattToPay += Whatshouldipayforkillo;


                        }
                        else if (Product_Fact_Listview.FocusedItem.SubItems[4].Text == "3")
                        {

                            float Whatshouldipayforkillo = float.Parse(Price_per_tonetxt.Text) * (float.Parse(Kilotxtbox.Text) / 33);
                            ListviewItem = new ListViewItem(new string[] { Kilotxtbox.Text + "شيكاره", ProductName, Math.Round(Whatshouldipayforkillo, 1) + "جنيه", Product_Fact_Listview.FocusedItem.SubItems[0].Text, (float.Parse(Kilotxtbox.Text) / 33).ToString() });
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
                string status = "";
                if (float.Parse(WhattoPay.Text) <= float.Parse(WhatHePay_txt.Text) - float.Parse(Discout_txt.Text))
                {
                    status = "خالص";
                }
                else
                {
                    status = "باقي";
                }
              int Bill_ID=  Database.CreateBillForAcustomer(CustomerID, status, PaymentMethodComob.Text, double.Parse(Discout_txt.Text));

                foreach (ListViewItem item in Fatora_Listview.Items)
                {
                    var Price = Regex.Split(item.SubItems[2].Text, @"[^0-9\.]+").Where(c => c != "." && c.Trim() != "");

                    Database.AddProductsToBill(Bill_ID
                        , int.Parse(item.SubItems[3].Text), 
                        float.Parse(item.SubItems[4].Text), float.Parse(Price.ToArray()[0]));



                    BillRepository.GeProducts().Add(new Bill()
                    {
                        ProductName = item.SubItems[1].Text,
                        Customer_Name = Customer_Name.Text,
                        Amont = item.SubItems[0].Text,
                        TotalPrice = Double.Parse(Price.ToArray()[0]),
                        Bill_ID = Bill_ID,
                        Discount=float.Parse(Discout_txt.Text)
                 

                    });
                }

                if (status == "باقي")
                {
                    Database.InserReminder(Bill_ID, Reminder);

                }

                Reports.Reports Ob = new Reports.Reports();
                Ob.Show();

                DoneFatora.Enabled = true;
               
                   
            }
            else
            {

                MessageBox.Show("تاكد من بعض البيانات !");
            }

            DoneFatora.Enabled = true;
        }

     
    }
}
