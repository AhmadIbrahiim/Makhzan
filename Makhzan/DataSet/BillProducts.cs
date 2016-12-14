
using System.Collections.Generic;

namespace Makhzan.DataSet
{
   public class Bill
    {
      
            public string ProductName { get; set; }
            public string Amont { get; set; }
            public double TotalPrice { get; set; }
            public int Bill_ID { get; set; }
            public double Discount { get; set; }
            public double Pricer_per_Tone { get; set; }
            public string Customer_Name { get; set; }
            public string Bill_State { get; set; }
            public double Reminder { get; set; }
            public string PaymentMethod { get; set; }
            public double WhatHePay { get; set; }
    }

    public class BillRepository
    {
        public static List<Bill> Products = new List<Bill>();

        public static List<Bill> GeProducts()
        {

            return Products;
        }

    }



}
