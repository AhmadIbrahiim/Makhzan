﻿using Makhzan.PresentaionLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Makhzan
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Paymentbtn_Click(object sender, EventArgs e)
        {

            new PaymentForm().Show();
            this.Hide();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
          //     MessageBox.Show();
        }

        private void Couponsbtn_Click(object sender, EventArgs e)
        {
          

            Coupons Ob = new Coupons();
            Ob.Show();
            this.Hide();
        }
    }
}
