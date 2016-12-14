
using System.Collections.Generic;

namespace Makhzan.DataSet
{
   public class BillDaily
    {
        public int Bill_ID { get; set; }
        public string Cust_Name { get; set; }
        public string What_He_Pay { get; set; }
        public string reminder { get; set; }
        public string TotalPriceForBill { get; set; }
        public string TotalEarn { get; set; }
        public string TotalReminder { get; set; }
        public string totalBills { get; set; }
            
    }

    public class BillDailyReposiotry
    {
        public static List<BillDaily> Products = new List<BillDaily>();

        public static List<BillDaily> GeProducts()
        {

            return Products;
        }

    }



}
