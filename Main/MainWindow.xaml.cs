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

            setDefaults();
             
         
        }

        public void setDefaults()
        {
            btnEditSaveInvoice.IsEnabled = false;
            cbItemList.ItemsSource = null;

            lbInvoiceNumber.Content = "Select or Create an Invoice";
            dpInvoiceDate.SelectedDate = null;
            txtTotalCost.Content = "";
            setReadOnlyMode();
        }

     

        public void setInvoice(int? selectedInvoice)
        {
            if(selectedInvoice == null)
            {
                setDefaults();
                return;
            }
            activeInvoice = new clsMainLogic((int)selectedInvoice);
            lbInvoiceNumber.Content = $"Invoice Number: {activeInvoice.getInvoiceNum()}";
            dpInvoiceDate.SelectedDate = activeInvoice.getInvoiceTime();
            dgInvoiceItemDisplay.ItemsSource = activeInvoice.getInvoiceItems();
            txtTotalCost.Content = activeInvoice.getTotalCost();
            cbItemList.ItemsSource = clsMainLogic.getItemList();
            btnEditSaveInvoice.Content = "Edit Invoice";
            btnEditSaveInvoice.IsEnabled = true;

        }

        private void btnSearchScreen_Click(object sender, RoutedEventArgs e)
        {
            wndSearchManger = new wndSearch();
            this.Hide();
            
            wndSearchManger.ShowDialog();

            setInvoice(wndSearch.SelectedInvoiceNum);
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
                dpInvoiceDate.IsEnabled = false; 

                int newInvoiceNumber = activeInvoice.newInvoice(dpInvoiceDate.SelectedDate, activeInvoice.getTotalCost());
                lbInvoiceNumber.Content = $"Invoice Number: {newInvoiceNumber}";
                newInvoice = false;

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
            dpInvoiceDate.IsEnabled = true;
            setInvoice(-1);
            lbInvoiceNumber.Content = $"Invoice Number: TBD";
            setEditMode();
            btnEditSaveInvoice.IsEnabled = false;
        }


        private void btnCancelChanges_Click(object sender, RoutedEventArgs e)
        {
            if(newInvoice)
            {
                setDefaults();
            }
            else
            {
                txtTotalCost.Content = activeInvoice.UpdateDataBase(false);
                setReadOnlyMode();
            }
           

        }

        private void cbItemList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            itemDetail x = (itemDetail)cbItemList.SelectedItem;
            if(x != null)
            {
                txtItemCost.Text = x.Cost.ToString();
            }
            else
            {
                txtItemCost.Text = "";
            }

        }

        private void dpInvoiceDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            btnEditSaveInvoice.IsEnabled = dgInvoiceItemDisplay.Items.Count > 0 && dpInvoiceDate.SelectedDate != null;
        }
    

        private void cmdDeleteItem_Click(object sender, RoutedEventArgs e)
        {
            bool noItems = false;
            itemDetail x = (itemDetail)dgInvoiceItemDisplay.SelectedItem;
            txtTotalCost.Content = activeInvoice.deleteItemFromInvoice(x, ref noItems);
           
            btnEditSaveInvoice.IsEnabled = !noItems && dpInvoiceDate.SelectedDate != null;         
        }

        private void btnAddItem_Click(object sender, RoutedEventArgs e)
        {
            itemDetail? selectedItem = cbItemList.SelectedValue as itemDetail;

            if (selectedItem != null)
            {
                txtTotalCost.Content = activeInvoice.newItem(selectedItem.ItemCode);
                btnEditSaveInvoice.IsEnabled = true && dpInvoiceDate.SelectedDate != null;
            }
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
            editMode = true;
            btnCancelChanges.IsEnabled = true;
            if(dgInvoiceItemDisplay.Items.Count < 0)
            {
                btnEditSaveInvoice.IsEnabled = false;
            }
        }

       
    }
}
