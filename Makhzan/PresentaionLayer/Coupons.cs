using Makhzan.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Makhzan.PresentaionLayer
{
    public partial class Coupons : Form
    {
        public Coupons()
        {
            InitializeComponent();
        }

        private void Coupons_Load(object sender, EventArgs e)
        {
            Fact_Combo.DataSource = Database.GetAllFacts();
            Fact_Combo.SelectedIndex = 0;

            Qt3Combo.DataSource = Database.GetAllFacts();
            Qt3Combo.SelectedIndex = 0;


        }

        private void Qt3Combo_SelectedIndexChanged(object sender, EventArgs e)
        {

            Qt3Product.DataSource = Database.Product_For_Fact(Qt3Combo.SelectedIndex);
        }

        private void PriceperUnit_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ttalPrice.Text = Convert.ToInt32(PriceperUnit.Text) * Convert.ToInt32(Qty.Text) + "";
            }
            catch
            {

            }
       }

        private void button2_Click(object sender, EventArgs e)
        {
            int[] amount = new int[3];
             amount = Database.GetCouponsAmontForFact(Qt3Combo.SelectedIndex + 1);

            if ( amount[0]<= int.Parse(ttalPrice.Text)&&ttalPrice.Text!="0")
            {
                DialogResult var = MessageBox.Show("هل انت متاكد من قطع كوبون بقيمة :" + ttalPrice.Text + " جنيه , من مصنع" + ":" + Qt3Combo.Text, "تاكيد العمليه", MessageBoxButtons.OKCancel);

                if (var == DialogResult.OK)
                {

                    Database.InsertCouponDeatails(Qt3Product.SelectedIndex + 1, int.Parse(ttalPrice.Text), int.Parse(PriceperUnit.Text), amount[1]);
          
                    MessageBox.Show("تم القطع");

                }
                else
                {
                    MessageBox.Show("تم الغاء عملية القطع");
                }
            }
            else
            {
                MessageBox.Show("تاكد من البيانات !");

            }
        }

        private void Qty_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ttalPrice.Text = Convert.ToInt32(PriceperUnit.Text) * Convert.ToInt32(Qty.Text) + "";
            }
            catch
            {

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            Database.InsertCouponForFact(int.Parse(textBox1.Text), Fact_Combo.SelectedIndex + 1);
            MessageBox.Show("تم ايداع المبلغ : " + textBox1.Text);
        }
    }
}
