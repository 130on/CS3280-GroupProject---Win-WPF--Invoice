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
        /// <summary>
        /// The window of itemManger.
        /// </summary>
        private wndItems wndItemManger;

        /// <summary>
        /// The search window object
        /// </summary>
        private wndSearch wndSearchManger;

        /// <summary>
        /// Whether the screen is in editMode
        /// </summary>
        private bool editMode = false;

        /// <summary>
        /// Whether the user is creating a new invoice
        /// </summary>
        private bool newInvoice = false;

        /// <summary>
        /// The activeInvoice at any given time can be null
        /// </summary>
        protected clsMainLogic? activeInvoice;

        /// <summary>
        /// Basic constructor for the main window, also runs setDefaults
        /// </summary>
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
        /// <summary>
        /// The default status of the application
        /// </summary>
        /// <exception cref="Exception"></exception>
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


        /// <summary>
        /// The selected invoice is found and updated on the screen.
        /// </summary>
        /// <param name="selectedInvoice">The selected invoice if null it returns to default screen</param>
        /// <exception cref="Exception"></exception>
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

        /// <summary>
        /// Opens the search screen and passes the selected invoice to setInvoice
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// Opens the item windows for editing. If changes have been made updates the combobox and the selected invoice.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnItemWindow_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                wndItemManger = new wndItems();

                this.Hide();
                wndItemManger.ShowDialog();
                if (wndItemManger.ItemModified)
                {
                    cbItemList.ItemsSource = clsMainLogic.getItemList();
                    if(activeInvoice != null)
                    {
                        txtTotalCost.Content = activeInvoice.UpdateDataBase(false);
                    }
                }

                this.Show();
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Saves the invoice or starts the ability to edit the invoice. Also updates the database.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Starts the invoice creation phase.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Cancels the changes done to the invoice, or cancels the invoice creation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancelChanges_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Enable editing the item list
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

        /// <summary>
        /// Deletes an item from the collection; not saved until save invoice is pressed. Also determines if save invoice can be edited 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdDeleteItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(activeInvoice != null)
                {
                    bool noItems = false;
                    itemDetail x = (itemDetail)dgInvoiceItemDisplay.SelectedItem;
                    txtTotalCost.Content = activeInvoice.deleteItemFromInvoice(x, ref noItems);

                    btnEditSaveInvoice.IsEnabled = !noItems && dpInvoiceDate.SelectedDate != null;
                }          
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Allows the user to add items to the invoice, not saved in database till save is pressed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                itemDetail? selectedItem = cbItemList.SelectedValue as itemDetail;

                if (selectedItem != null && activeInvoice != null)
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


        /// <summary>
        /// Sets the UI to readonly mode (can't edit invoice)
        /// </summary>
        /// <exception cref="Exception"></exception>
        private void setReadOnlyMode()
        {

            try
            {
                btnEditSaveInvoice.Content = "Edit Invoice";
                editMode = false;
                btnAddItem.IsEnabled = false;
                btnCancelChanges.IsEnabled = false;
                btnItemWindow.IsEnabled = true;

            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }


        /// <summary>
        /// Sets the UI to edit mode (can edit invoice)
        /// </summary>
        /// <exception cref="Exception"></exception>
        private void setEditMode()
        {
            try
            {
                btnEditSaveInvoice.Content = "Save Invoice";
                btnAddItem.IsEnabled = true;
                editMode = true;
                btnCancelChanges.IsEnabled = true;
                btnItemWindow.IsEnabled = false;

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
        /// Displays a message with info about the error
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
