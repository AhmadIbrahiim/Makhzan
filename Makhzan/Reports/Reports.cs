using Makhzan.DataSet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Makhzan.Reports
{
    public partial class Reports : Form
    {
        public Reports()
        {
            InitializeComponent();
        }

        private void Reports_Load(object sender, EventArgs e)
        {

            reportViewer1.LocalReport.DataSources.Clear(); //clear report
            reportViewer1.LocalReport.Refresh();
            reportViewer1.DataBindings.Clear();
            List<Bill> list = BillRepository.GeProducts(); //get list of students
            reportViewer1.LocalReport.ReportEmbeddedResource = "Makhzan.Reports.Bill.rdlc"; // bind reportviewer with .rdlc
            Microsoft.Reporting.WinForms.ReportDataSource dataset = new Microsoft.Reporting.WinForms.ReportDataSource("DataSet1", list); // set the datasource
            reportViewer1.LocalReport.DataSources.Add(dataset);
            dataset.Value = list;
            this.reportViewer1.RefreshReport();
        }
    }
}
