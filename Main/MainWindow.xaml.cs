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
        private wndItems wndItemManger;
        private wndSearch wndSearchManger;
        private bool editMode = true;
        public MainWindow()
        {

            InitializeComponent();
            Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;

            //Test Statement to test sql access 
            clsMainLogic.testSQLStatement();
            btnEditSaveInvoice.Content = "Edit Invoice";

        }

        private void btnSearchScreen_Click(object sender, RoutedEventArgs e)
        {
            wndSearchManger = new wndSearch();
            this.Hide();
            
            wndSearchManger.ShowDialog();
            //check clsSearchLogic if there is a change has been made. 
            //And or get the return variable from wndSearch

            this.Show();
        }

        private void btnItemWindow_Click(object sender, RoutedEventArgs e)
        {
            wndItemManger = new wndItems();

            this.Hide();
            wndItemManger.ShowDialog();
            //check clsItemLogic if there is a change has been made. if it is a static var
             //And or get the return variable from wndItem


            this.Show();
        }

        private void btnEditSaveInvoice_Click(object sender, RoutedEventArgs e)
        {
            if (editMode)
            {
                btnEditSaveInvoice.Content = "Save Invoice";
                dpInvoiceDate.IsEnabled = true;
                editMode = true;
                
            }
            else if(!editMode)
            {
                dpInvoiceDate.IsEnabled = false;
                btnEditSaveInvoice.Content = "Edit Invoice";
                editMode = false;
            }
        }

        private void btnNewInvoice_Click(object sender, RoutedEventArgs e)
        {
            btnEditSaveInvoice.IsEnabled = true;
        }


        private void btnCancelChanges_Click(object sender, RoutedEventArgs e)
        {
            btnEditSaveInvoice.Content = "Edit Invoice";

        }
    }
}
