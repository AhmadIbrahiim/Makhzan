namespace Makhzan
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.Paymentbtn = new System.Windows.Forms.Button();
            this.Storagebtn = new System.Windows.Forms.Button();
            this.Couponsbtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Paymentbtn
            // 
            this.Paymentbtn.Font = new System.Drawing.Font("Adobe Arabic", 72F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Paymentbtn.Location = new System.Drawing.Point(48, 184);
            this.Paymentbtn.Name = "Paymentbtn";
            this.Paymentbtn.Size = new System.Drawing.Size(416, 184);
            this.Paymentbtn.TabIndex = 0;
            this.Paymentbtn.Text = "مبيعات";
            this.Paymentbtn.UseVisualStyleBackColor = true;
            this.Paymentbtn.Click += new System.EventHandler(this.Paymentbtn_Click);
            // 
            // Storagebtn
            // 
            this.Storagebtn.Font = new System.Drawing.Font("Adobe Arabic", 72F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Storagebtn.Location = new System.Drawing.Point(530, 184);
            this.Storagebtn.Name = "Storagebtn";
            this.Storagebtn.Size = new System.Drawing.Size(416, 184);
            this.Storagebtn.TabIndex = 1;
            this.Storagebtn.Text = "مخزن";
            this.Storagebtn.UseVisualStyleBackColor = true;
            // 
            // Couponsbtn
            // 
            this.Couponsbtn.Font = new System.Drawing.Font("Adobe Arabic", 72F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Couponsbtn.Location = new System.Drawing.Point(276, 448);
            this.Couponsbtn.Name = "Couponsbtn";
            this.Couponsbtn.Size = new System.Drawing.Size(416, 184);
            this.Couponsbtn.TabIndex = 2;
            this.Couponsbtn.Text = "كوبونات";
            this.Couponsbtn.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 661);
            this.Controls.Add(this.Couponsbtn);
            this.Controls.Add(this.Storagebtn);
            this.Controls.Add(this.Paymentbtn);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "اختار الاتجاه ";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Paymentbtn;
        private System.Windows.Forms.Button Storagebtn;
        private System.Windows.Forms.Button Couponsbtn;
    }
}

