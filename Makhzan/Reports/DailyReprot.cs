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

namespace Makhzan
{
    public partial class DailyReprot : Form
    {
        public DailyReprot()
        {
            InitializeComponent();
        }

        private void DailyReprot_Load(object sender, EventArgs e)
        {
            reportViewer1.LocalReport.DataSources.Clear(); //clear report
            reportViewer1.LocalReport.Refresh();
            reportViewer1.DataBindings.Clear();
            List<BillDaily> list = BillDailyReposiotry.GeProducts(); //get list of students
            reportViewer1.LocalReport.ReportEmbeddedResource = "Makhzan.Reports.Daily.rdlc"; ; // bind reportviewer with .rdlc
            Microsoft.Reporting.WinForms.ReportDataSource dataset = new Microsoft.Reporting.WinForms.ReportDataSource("DataSet1", list); // set the datasource
            reportViewer1.LocalReport.DataSources.Add(dataset);
            dataset.Value = list;
            this.reportViewer1.RefreshReport();
        }
    }
}
