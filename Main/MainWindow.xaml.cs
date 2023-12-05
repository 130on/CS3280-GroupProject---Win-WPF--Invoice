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
        private bool editMode = false;
        private bool newInvoice = false;

        clsMainLogic? activeInvoice;
       // private Button cmdDeleteItem;

        public MainWindow()
        {

            InitializeComponent();
            Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;

            //Test Statement to test sql access 
            btnEditSaveInvoice.Content = "Edit Invoice";
            txtItemCost.Text = "";
            cbItemList.ItemsSource = clsMainLogic.getItemList();
            txtTotalCost.Content = 0;
            activeInvoice = new clsMainLogic(5000);
            lbInvoiceNumber.Content += activeInvoice.getInvoiceNum();
            dpInvoiceDate.SelectedDate = activeInvoice.getInvoiceTime();
            dgInvoiceItemDisplay.ItemsSource = activeInvoice.getInvoiceItems();
            txtTotalCost.Content = activeInvoice.getTotalCost();
         
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
            if (newInvoice)
            {

            }
            txtTotalCost.Content = activeInvoice.UpdateDataBase(true);

            if (!editMode)
            {
                setEditMode();

            }
            else if(editMode)
            {

                setReadOnlyMode();
            }

        }

        private void btnNewInvoice_Click(object sender, RoutedEventArgs e)
        {
            newInvoice = true;
            invoiceSelected();
        }


        private void btnCancelChanges_Click(object sender, RoutedEventArgs e)
        {
            txtTotalCost.Content = activeInvoice.UpdateDataBase(false);
            setReadOnlyMode();

        }

        private void cbItemList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            itemDetail x = (itemDetail)cbItemList.SelectedItem;
            txtItemCost.Text = x.Cost.ToString();
        }

        private void dpInvoiceDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            dpInvoiceDate.IsEnabled = false;
        }

        private void cmdDeleteItem_Click(object sender, RoutedEventArgs e)
        {
            itemDetail x = (itemDetail)dgInvoiceItemDisplay.SelectedItem;
            txtTotalCost.Content = activeInvoice.deleteItemFromInvoice(x);
        }

        private void btnAddItem_Click(object sender, RoutedEventArgs e)
        {
            itemDetail? selectedItem = cbItemList.SelectedValue as itemDetail;

            if (selectedItem != null)
            {
                txtTotalCost.Content = activeInvoice.newItem(selectedItem.ItemCode);
            }
        }


        private void invoiceSelected()
        {
            btnEditSaveInvoice.IsEnabled = true;
            setReadOnlyMode();
        }
        private void setReadOnlyMode()
        {
            btnEditSaveInvoice.Content = "Edit Invoice";
            editMode = false;
            btnAddItem.IsEnabled = false;
            btnCancelChanges.IsEnabled = false;
        }



        private void setEditMode()
        {

            btnEditSaveInvoice.Content = "Save Invoice";
            btnAddItem.IsEnabled = true;
            btnAddItem.IsEnabled = true;
            editMode = true;
            btnCancelChanges.IsEnabled = true;
        }
    }
}
