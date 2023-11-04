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

            //Test Statement to test sql access 
            DataSet dsInvoice = new();
            int count = 0;   //Number of return values
            string sSQL = "SELECT * FROM Invoices";
            dsInvoice = clsDataAccess.ExecuteSQLStatement(sSQL, ref count);


            //Testing repo access
            int x = 4;
            int y = 5;
            for(int i  = 0; i < x; i++)
            {
                y += i*2;
            }
        }
    }
}
