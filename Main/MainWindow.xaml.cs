using GroupAssignmentAlonColetonWannes.Common;
using GroupAssignmentAlonColetonWannes.Items;
using GroupAssignmentAlonColetonWannes.Main;
using GroupAssignmentAlonColetonWannes.Search;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GroupAssignmentAlonColetonWannes
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;

            //Test Statement to test sql access 
            DataSet dsInvoice = new();
            int count = 0;   //Number of return values
            string sSQL = "SELECT * FROM Invoices";
            dsInvoice = clsDataAccess.ExecuteSQLStatement(sSQL, ref count);






            clsSearchSQL.loadInvoices();

            clsSearchSQL.loadInvoices(5000, null, null);
            DateTime x = DateTime.Now;
            clsSearchSQL.loadInvoices(null, x, null);

            clsSearchSQL.loadInvoices(5000, x, null);

            clsSearchSQL.loadInvoices(null, x, 120);

            int z = 0;

            clsItemsSQL.getInvoicesWithItemCode("A", ref z);


            Dictionary<int, string> invoiceItems = clsMainSQL.getInvoiceItems(5000);
        }
    }
}
