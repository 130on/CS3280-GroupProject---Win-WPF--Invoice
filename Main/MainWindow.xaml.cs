using GroupAssignmentAlonColetonWannes.Common;
using GroupAssignmentAlonColetonWannes.Items;
using GroupAssignmentAlonColetonWannes.Main;
using GroupAssignmentAlonColetonWannes.Search;
using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

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



            try
            {

                InitializeComponent();
                Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
                cbItemList.ItemsSource = clsMainLogic.getItemList();

                setDefaults();
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        public void setDefaults()
        {

            try
            {
                btnEditSaveInvoice.IsEnabled = false;
                cbItemList.SelectedIndex = -1;
                dgInvoiceItemDisplay.ItemsSource = null;
                lbInvoiceNumber.Content = "Select or Create an Invoice";
                dpInvoiceDate.SelectedDate = null;
                txtTotalCost.Content = "";
                setReadOnlyMode();
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }



        public void setInvoice(int? selectedInvoice)
        {

            try
            {
                if (selectedInvoice == null)
                {
                    setDefaults();
                    return;
                }
                activeInvoice = new clsMainLogic((int)selectedInvoice);
                lbInvoiceNumber.Content = $"Invoice Number: {activeInvoice.getInvoiceNum()}";
                dpInvoiceDate.SelectedDate = activeInvoice.getInvoiceTime();
                dgInvoiceItemDisplay.ItemsSource = activeInvoice.getInvoiceItems();
                txtTotalCost.Content = activeInvoice.getTotalCost();
                btnEditSaveInvoice.IsEnabled = true;
                setReadOnlyMode();
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }

        }

        private void btnSearchScreen_Click(object sender, RoutedEventArgs e)
        {


            try
            {

                wndSearchManger = new wndSearch();
                this.Hide();

                wndSearchManger.ShowDialog();

                setInvoice(wndSearch.SelectedInvoiceNum);

                this.Show();
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void btnItemWindow_Click(object sender, RoutedEventArgs e)
        {

            try
            {

                wndItemManger = new wndItems();

                this.Hide();
                wndItemManger.ShowDialog();
                if (wndItemManger.ItemModified)
                {
                    clsMainLogic.getItemList();
                    cbItemList.ItemsSource = clsMainLogic.getItemList();


                }

                this.Show();
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void btnEditSaveInvoice_Click(object sender, RoutedEventArgs e)
        {

            try
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
                else if (editMode)
                {
                    setReadOnlyMode();
                }

            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void btnNewInvoice_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                newInvoice = true;
                dpInvoiceDate.IsEnabled = true;
                setInvoice(-1);
                lbInvoiceNumber.Content = $"Invoice Number: TBD";
                setEditMode();
                btnEditSaveInvoice.IsEnabled = false;

            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }


        private void btnCancelChanges_Click(object sender, RoutedEventArgs e)
        {

            try
            {

                if (newInvoice)
                {
                    setDefaults();
                }
                else
                {
                    txtTotalCost.Content = activeInvoice.UpdateDataBase(false);
                    setReadOnlyMode();
                }
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }

        }

        private void cbItemList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            try
            {
                itemDetail selectedItem = (itemDetail)cbItemList.SelectedItem;
                if (selectedItem != null)
                {
                    txtItemCost.Text = selectedItem.Cost.ToString();
                }
                else
                {
                    txtItemCost.Text = "";
                }

            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void dpInvoiceDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                btnEditSaveInvoice.IsEnabled = dgInvoiceItemDisplay.Items.Count > 0 && dpInvoiceDate.SelectedDate != null;


            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }


        private void cmdDeleteItem_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                bool noItems = false;
                itemDetail x = (itemDetail)dgInvoiceItemDisplay.SelectedItem;
                txtTotalCost.Content = activeInvoice.deleteItemFromInvoice(x, ref noItems);

                btnEditSaveInvoice.IsEnabled = !noItems && dpInvoiceDate.SelectedDate != null;

            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void btnAddItem_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                itemDetail? selectedItem = cbItemList.SelectedValue as itemDetail;

                if (selectedItem != null)
                {
                    txtTotalCost.Content = activeInvoice.newItem(selectedItem.ItemCode);
                    btnEditSaveInvoice.IsEnabled = true && dpInvoiceDate.SelectedDate != null;
                }

            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }



        private void setReadOnlyMode()
        {

            try
            {
                btnEditSaveInvoice.Content = "Edit Invoice";
                editMode = false;
                btnAddItem.IsEnabled = false;
                btnCancelChanges.IsEnabled = false;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }



        private void setEditMode()
        {



            try
            {
                btnEditSaveInvoice.Content = "Save Invoice";
                btnAddItem.IsEnabled = true;
                editMode = true;
                btnCancelChanges.IsEnabled = true;
                if (dgInvoiceItemDisplay.Items.Count < 0)
                {
                    btnEditSaveInvoice.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// displays a message with info about the error
        /// </summary>
        /// <param name="sClass"></param>
        /// <param name="sMethod"></param>
        /// <param name="sMessage"></param>
        private void HandleError(string sClass, string sMethod, string sMessage)
        {
            try
            {
                MessageBox.Show(sClass + "." + sMethod + " -> " + sMessage);
            }
            catch (System.Exception ex)
            {
                System.IO.File.AppendAllText(@"C:\Error.txt", Environment.NewLine + "HandleError Exception: " + ex.Message);
            }
        }
    }
}
